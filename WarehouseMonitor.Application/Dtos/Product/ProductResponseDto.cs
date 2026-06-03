namespace WarehouseMonitor.Application.Dtos.Product;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CurrentStock { get; set; }
    public int ReorderPoint { get; set; }
    public int SafetyStock { get; set; }
    public decimal? UnitPrice { get; set; }
    public bool IsActive { get; set; }
    public bool NeedsReorder => CurrentStock <= ReorderPoint;
}