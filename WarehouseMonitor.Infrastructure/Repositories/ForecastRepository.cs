using Microsoft.EntityFrameworkCore;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.Data;

namespace WarehouseMonitor.Infrastructure.Repositories;

public class ForecastRepository : GenericRepository<Forecast>, IForecastRepository
{
    public ForecastRepository(AppDbContext context) : base(context) { }

    public async Task<Forecast?> GetLatestForProductAsync(Guid productId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(f => f.ProductId == productId)
            .OrderByDescending(f => f.GeneratedAt)
            .FirstOrDefaultAsync(cancellationToken);
}