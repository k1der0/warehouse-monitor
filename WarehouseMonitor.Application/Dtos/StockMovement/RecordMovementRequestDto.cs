namespace WarehouseMonitor.Application.Dtos.StockMovement;

public class RecordMovementRequestDto
{
    public Guid ProductId { get; set; }
    public Guid? WarehouseId { get; set; }     // если несколько складов
    public int Quantity { get; set; }          // положительное = приход, отрицательное = расход
    public string MovementType { get; set; } = string.Empty; // "Receipt", "Sale", "Adjustment"
    public string? Reference { get; set; }     // номер заказа/накладной
    public string? Note { get; set; }
}