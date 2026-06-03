namespace WarehouseMonitor.Application.Dtos.Product;

public class ProductBriefResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
}