using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.Data;

namespace WarehouseMonitor.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        
        // Инициализация всех репозиториев
        Products = new ProductRepository(context);
        Inventories = new InventoryRepository(context);
        SalesHistories = new SalesHistoryRepository(context);
        Forecasts = new ForecastRepository(context);
        Warehouses = new WarehouseRepository(context);
        StockMovements = new StockMovementRepository(context);
    }

    public IProductRepository Products { get; }
    public IInventoryRepository Inventories { get; }
    public ISalesHistoryRepository SalesHistories { get; }
    public IForecastRepository Forecasts { get; }
    public IWarehouseRepository Warehouses { get; }
    public IStockMovementRepository StockMovements { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            await _transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            await _transaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}