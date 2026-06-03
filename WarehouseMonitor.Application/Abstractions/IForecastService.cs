namespace WarehouseMonitor.Application.Abstractions;

public interface IForecastService
{
    Task<List<Domain.Entities.Forecast>> GenerateForecastForProductAsync(Guid productId, int forecastDays = 30, CancellationToken cancellationToken = default);
}