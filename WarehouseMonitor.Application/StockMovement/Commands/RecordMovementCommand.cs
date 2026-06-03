using MediatR;

namespace WarehouseMonitor.Application.StockMovement.Commands;

public record RecordMovementCommand(
    Guid ProductId,
    int Quantity,
    string MovementType,   // "Receipt", "Sale", "Adjustment"
    string? Reference = null,
    string? Note = null,
    Guid? WarehouseId = null
) : IRequest<Guid>;