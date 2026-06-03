using MediatR;

namespace WarehouseMonitor.Application.Warehouse.Commands;

public record DeleteWarehouseCommand(Guid Id) : IRequest;