using Microsoft.EntityFrameworkCore;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.Data;

namespace WarehouseMonitor.Infrastructure.Repositories;

public class SalesHistoryRepository : GenericRepository<SalesHistory>, ISalesHistoryRepository
{
    public SalesHistoryRepository(AppDbContext context) : base(context) { }

    public async Task<IReadOnlyList<SalesHistory>> GetSalesForProductAsync(Guid productId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(s => s.ProductId == productId && s.SaleDate >= startDate && s.SaleDate <= endDate)
            .OrderBy(s => s.SaleDate)
            .ToListAsync(cancellationToken);
}