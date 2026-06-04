using WarehouseMonitor.Domain.ValueObjects;

namespace WarehouseMonitor.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CurrentStock { get; set; }
    public int ReorderPoint { get; set; }
    public int SafetyStock { get; set; }
    public bool IsActive { get; set; } = true;
    public Money? UnitPrice { get; set; }

    // Навигационные свойства
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    public ICollection<InventoryLevel> InventoryLevels { get; set; } = new List<InventoryLevel>();
    public ICollection<SalesHistory> SalesHistory { get; set; } = new List<SalesHistory>();
    public ICollection<Forecast> Forecasts { get; set; } = new List<Forecast>();
}