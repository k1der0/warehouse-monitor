using MediatR;
using WarehouseMonitor.Application.Abstractions;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.Forecast.Commands;


/// <summary>
/// Обработчик команды генерации прогноза
/// </summary>
public class GenerateForecastCommandHandler : IRequestHandler<GenerateForecastCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IForecastService _forecastService;

    public GenerateForecastCommandHandler(IUnitOfWork unitOfWork, IForecastService forecastService)
    {
        _unitOfWork = unitOfWork;
        _forecastService = forecastService;
    }

    public async Task<bool> Handle(GenerateForecastCommand request, CancellationToken cancellationToken)
    {
        // 1. Проверяем существование товара
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException($"Товар с ID {request.ProductId} не найден");

        // 2. Генерируем прогнозы через ML.NET сервис
        var forecasts = await _forecastService.GenerateForecastForProductAsync(
            request.ProductId, 
            request.ForecastDays, 
            cancellationToken);

        // 3. Если прогнозы не сгенерированы (недостаточно данных)
        if (forecasts == null || !forecasts.Any())
            return false;

        // 4. Удаляем старые прогнозы для этого товара
        var existingForecasts = await _unitOfWork.Forecasts
            .FindAsync(f => f.ProductId == request.ProductId, cancellationToken);
        
        foreach (var oldForecast in existingForecasts)
            _unitOfWork.Forecasts.Delete(oldForecast);

        // 5. Добавляем новые прогнозы
        foreach (var newForecast in forecasts)
            await _unitOfWork.Forecasts.AddAsync(newForecast, cancellationToken);

        // 6. Сохраняем изменения в базе данных
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}