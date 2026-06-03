using Microsoft.EntityFrameworkCore;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.Data;

namespace WarehouseMonitor.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);

    public async Task<IReadOnlyList<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
        => await _dbSet.Where(p => p.IsActive).ToListAsync(cancellationToken);
}