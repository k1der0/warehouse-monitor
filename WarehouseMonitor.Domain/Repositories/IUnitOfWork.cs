using System;
using System.Threading;
using System.Threading.Tasks;

namespace WarehouseMonitor.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IInventoryRepository Inventories { get; }
    ISalesHistoryRepository SalesHistories { get; }
    IForecastRepository Forecasts { get; }
    IWarehouseRepository Warehouses { get; }
    IStockMovementRepository StockMovements { get; }
    
    // Методы управления транзакциями и сохранением
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}