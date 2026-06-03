using MediatR;

namespace WarehouseMonitor.Application.Warehouse.Commands;

public record CreateWarehouseCommand(
    string Name,
    string Location
) : IRequest<Guid>;