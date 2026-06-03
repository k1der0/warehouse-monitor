using MediatR;
using WarehouseMonitor.Application.Dtos.StockMovement;

namespace WarehouseMonitor.Application.StockMovements.Queries;

public record GetMovementsByProductQuery(Guid ProductId) : IRequest<IReadOnlyList<StockMovementResponseDto>>;