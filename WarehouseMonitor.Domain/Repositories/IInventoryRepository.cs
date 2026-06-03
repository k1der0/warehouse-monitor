using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Domain.Repositories;

public interface IInventoryRepository : IRepository<InventoryLevel>
{
    Task<InventoryLevel?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<InventoryLevel>> GetLowStockAsync(int threshold, CancellationToken cancellationToken = default);
}