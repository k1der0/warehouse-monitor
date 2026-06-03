using Microsoft.EntityFrameworkCore;
using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }
    public DbSet<InventoryLevel> InventoryLevels { get; set; }
    public DbSet<SalesHistory> SalesHistories { get; set; }
    public DbSet<Forecast> Forecasts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Здесь могут быть дополнительные настройки, если нужно
        base.OnModelCreating(modelBuilder);
    }
}