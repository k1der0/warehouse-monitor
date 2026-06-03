namespace WarehouseMonitor.Application.Dtos.StockMovement;

public class StockMovementResponseDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;
    public DateTime MovementDate { get; set; }
    public int Quantity { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public string? Note { get; set; }
}