using WarehouseMonitor.Application.Abstractions;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Infrastructure.Services;

public class ForecastService : IForecastService
{
    private readonly IUnitOfWork _unitOfWork;

    public ForecastService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Forecast>> GenerateForecastForProductAsync(Guid productId, int forecastDays, CancellationToken cancellationToken)
    {
        // 1. Получаем историю продаж за последние 90 дней
        var startDate = DateTime.UtcNow.AddDays(-90);
        var salesHistory = await _unitOfWork.SalesHistories
            .GetSalesForProductAsync(productId, startDate, DateTime.UtcNow, cancellationToken);

        if (!salesHistory.Any())
            return new List<Forecast>();

        // 2. Группируем по дням и получаем дневные продажи
        var dailySales = salesHistory
            .GroupBy(x => x.SaleDate.Date)
            .Select(g => (Date: g.Key, Quantity: (double)g.Sum(x => x.QuantitySold)))
            .OrderBy(x => x.Date)
            .ToList();

        if (dailySales.Count < 7)
            return new List<Forecast>(); // недостаточно данных

        // 3. Прогнозирование методом экспоненциального сглаживания с трендом (модель Хольта)
        var quantities = dailySales.Select(x => x.Quantity).ToArray();
        var alpha = 0.3;   // коэффициент сглаживания уровня
        var beta = 0.1;    // коэффициент сглаживания тренда

        double level = quantities[0];
        double trend = quantities[1] - quantities[0];

        for (int i = 1; i < quantities.Length; i++)
        {
            double prevLevel = level;
            level = alpha * quantities[i] + (1 - alpha) * (level + trend);
            trend = beta * (level - prevLevel) + (1 - beta) * trend;
        }

        // Генерируем прогноз на forecastDays дней
        var result = new List<Forecast>();
        for (int i = 1; i <= forecastDays; i++)
        {
            double predicted = level + i * trend;
            predicted = Math.Max(0, predicted); // не может быть отрицательным
            var forecastDate = DateTime.UtcNow.Date.AddDays(i);
            result.Add(new Forecast
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                ForecastDate = forecastDate,
                PredictedDemand = (int)Math.Round(predicted),
                LowerBound = (int)Math.Round(predicted * 0.8),  // примерные границы
                UpperBound = (int)Math.Round(predicted * 1.2),
                GeneratedAt = DateTime.UtcNow
            });
        }

        return result;
    }
}