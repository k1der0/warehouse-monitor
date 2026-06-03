using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Domain.Repositories;

public interface IStockMovementRepository : IRepository<StockMovement>
{
    // Пример специфичного метода:
    // Task<IReadOnlyList<StockMovement>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}