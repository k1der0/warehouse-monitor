using Microsoft.EntityFrameworkCore;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.Data;

namespace WarehouseMonitor.Infrastructure.Repositories;

public class InventoryRepository : GenericRepository<InventoryLevel>, IInventoryRepository
{
    public InventoryRepository(AppDbContext context) : base(context) { }

    public async Task<InventoryLevel?> GetByProductAndWarehouseAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default)
        => await _dbSet
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.WarehouseId == warehouseId, cancellationToken);
    public async Task<IReadOnlyList<InventoryLevel>> GetLowStockAsync(int threshold, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(i => i.QuantityOnHand <= threshold)
            .Include(i => i.Product)
            .ToListAsync(cancellationToken);
}