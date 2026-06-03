using WarehouseMonitor.Domain.ValueObjects;

namespace WarehouseMonitor.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string Sku { get; set; } = string.Empty; // артикул
    public int CurrentStock { get; set; } 
    public string? Description { get; set; } =  string.Empty;
    public Money? UnitPrice { get; set; }       // цена за единицу, опционально
    public int ReorderPoint { get; set; }      // порог для заказа (мин. остаток)
    public int SafetyStock { get; set; }       // страховой запас
    public bool IsActive { get; set; } = true;
    
    // Навигационные свойства
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    public ICollection<InventoryLevel> InventoryLevels { get; set; } = new List<InventoryLevel>();
    public ICollection<SalesHistory> SalesHistory { get; set; } = new List<SalesHistory>();
    public ICollection<Forecast> Forecasts { get; set; } = new List<Forecast>();
}