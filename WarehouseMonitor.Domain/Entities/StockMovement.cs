namespace WarehouseMonitor.Domain.Entities;

public class StockMovement
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public DateTime MovementDate { get; set; }
    public int Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public string? Note { get; set; }

    // Навигационные свойства
    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}