using MediatR;

namespace WarehouseMonitor.Application.SalesHistory.Commands;

public record AddSalesHistoryCommand(
    Guid ProductId,
    DateTime SaleDate,
    int QuantitySold,
    decimal? Revenue = null
) : IRequest<Guid>;