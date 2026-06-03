namespace WarehouseMonitor.Application.Dtos.Product;

public class CreateProductRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;          // уникальный артикул
    public string? Description { get; set; }
    public int CurrentStock { get; set; }                    // начальный остаток
    public int ReorderPoint { get; set; }                    // порог заказа
    public int SafetyStock { get; set; }                     // страховой запас
    public decimal? UnitPrice { get; set; }                  // опционально
}