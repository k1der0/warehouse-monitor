using Microsoft.EntityFrameworkCore;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.Data;

namespace WarehouseMonitor.Infrastructure.Repositories;

public class StockMovementRepository : GenericRepository<StockMovement>, IStockMovementRepository
{
    public StockMovementRepository(AppDbContext context) : base(context) { }
    
    // Если в интерфейсе IStockMovementRepository есть дополнительные методы (например, GetMovementsByProductIdAsync)
    // реализуйте их здесь.
}