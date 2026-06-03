using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Domain.Repositories;

public interface IForecastRepository : IRepository<Forecast>
{
    Task<Forecast?> GetLatestForProductAsync(Guid productId, CancellationToken cancellationToken = default);
}