using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Domain.Repositories;

public interface ISalesHistoryRepository : IRepository<SalesHistory>
{
    Task<IReadOnlyList<SalesHistory>> GetSalesForProductAsync(Guid productId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}