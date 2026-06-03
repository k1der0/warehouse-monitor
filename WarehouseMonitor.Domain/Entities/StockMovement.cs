namespace WarehouseMonitor.Domain.Entities;

public class StockMovement
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public DateTime MovementDate { get; set; }
    public int Quantity { get; set; }           // положительное = приход, отрицательное = расход
    public string MovementType { get; set; } = string.Empty; // "Receipt", "Sale", "Return", "Adjustment"
    public string? Reference { get; set; }      // номер заказа, накладной
    public string? Note { get; set; }
    
    // Навигация
    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}