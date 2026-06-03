using MediatR;

namespace WarehouseMonitor.Application.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest;