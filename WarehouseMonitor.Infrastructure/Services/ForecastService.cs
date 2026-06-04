using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using WarehouseMonitor.Application.Abstractions;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.ML;

namespace WarehouseMonitor.Infrastructure.Services;

public class ForecastService : IForecastService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly MLContext _mlContext;

    public ForecastService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mlContext = new MLContext();
    }

    public async Task<List<Forecast>> GenerateForecastForProductAsync(
        Guid productId, int forecastDays, CancellationToken cancellationToken)
    {
        // 1. Получаем историю продаж за последний год
        var startDate = DateTime.UtcNow.AddDays(-365);
        var salesHistory = await _unitOfWork.SalesHistories
            .GetSalesForProductAsync(productId, startDate, DateTime.UtcNow, cancellationToken);

        if (!salesHistory.Any() || salesHistory.Count < 30)
            return new List<Forecast>();

        // 2. Подготавливаем данные для ML.NET
        var salesData = salesHistory
            .GroupBy(x => x.SaleDate.Date)
            .Select(g => new SalesData 
            { 
                Date = g.Key, 
                Quantity = (float)g.Sum(x => x.QuantitySold) 
            })
            .OrderBy(x => x.Date)
            .ToList();

        // 3. Загружаем данные в IDataView
        IDataView dataView = _mlContext.Data.LoadFromEnumerable(salesData);

        // 4. Создаем pipeline прогнозирования SSA
        var forecastingPipeline = _mlContext.Forecasting.ForecastBySsa(
            outputColumnName: nameof(SalesPrediction.ForecastedQuantities),
            inputColumnName: nameof(SalesData.Quantity),
            windowSize: 7,
            seriesLength: 30,
            trainSize: salesData.Count,
            horizon: forecastDays,
            confidenceLevel: 0.95f,
            confidenceLowerBoundColumn: nameof(SalesPrediction.LowerBound),
            confidenceUpperBoundColumn: nameof(SalesPrediction.UpperBound)
        );

        // 5. Обучаем модель
        var transformer = forecastingPipeline.Fit(dataView);
        
        // 6. Создаем движок прогнозирования
        var forecastEngine = transformer.CreateTimeSeriesEngine<SalesData, SalesPrediction>(_mlContext);
        
        // 7. Получаем прогноз
        var prediction = forecastEngine.Predict();
        
        // 8. Сохраняем модель (опционально)
        var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "Models", $"forecast_{productId}.zip");
        Directory.CreateDirectory(Path.GetDirectoryName(modelPath) ?? "Models");
        forecastEngine.CheckPoint(_mlContext, modelPath);
        
        // 9. Формируем результат
        var forecasts = new List<Forecast>();
        for (int i = 0; i < forecastDays && i < prediction.ForecastedQuantities.Length; i++)
        {
            var forecastDate = DateTime.UtcNow.Date.AddDays(i + 1);
            forecasts.Add(new Forecast
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                ForecastDate = forecastDate,
                PredictedDemand = (int)Math.Round(prediction.ForecastedQuantities[i]),
                LowerBound = (int)Math.Round(prediction.LowerBound?[i] ?? 0),
                UpperBound = (int)Math.Round(prediction.UpperBound?[i] ?? 0),
                GeneratedAt = DateTime.UtcNow
            });
        }

        return forecasts;
    }
}