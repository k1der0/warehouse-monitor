namespace WarehouseMonitor.Domain.Entities;

public class InventoryLevel
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public int QuantityOnHand { get; set; }      // текущий остаток
    public DateTime LastUpdated { get; set; }
    
    public Product Product { get; set; } = null!;
    public Warehouse Warehouse { get; set; } = null!;
}