using MediatR;
using WarehouseMonitor.Application.Dtos.SalesHistory;

namespace WarehouseMonitor.Application.SalesHistory.Queries;

public record GetSalesHistoryForProductQuery(
    Guid ProductId,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<IReadOnlyList<SalesHistoryResponseDto>>;