using Microsoft.EntityFrameworkCore;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.Data;

namespace WarehouseMonitor.Infrastructure.Repositories;

public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(AppDbContext context) : base(context) { }
    
    // Добавьте специфические методы, если они есть в IWarehouseRepository
    // Например:
    // public async Task<Warehouse?> GetByNameAsync(string name, CancellationToken ct = default)
    //     => await _dbSet.FirstOrDefaultAsync(w => w.Name == name, ct);
}