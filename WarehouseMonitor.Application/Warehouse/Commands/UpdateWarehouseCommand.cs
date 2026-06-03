using MediatR;

namespace WarehouseMonitor.Application.Warehouse.Commands;

public record UpdateWarehouseCommand(
    Guid Id,
    string Name,
    string Location
) : IRequest;