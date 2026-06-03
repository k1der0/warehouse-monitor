using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Domain.Repositories;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    // Можно добавить специфичные методы, например:
    // Task<Warehouse?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}