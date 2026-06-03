using MediatR;
using WarehouseMonitor.Application.Dtos.Warehouse;

namespace WarehouseMonitor.Application.Warehouse.Queries;

public record GetWarehouseByIdQuery(Guid Id) : IRequest<WarehouseResponseDto?>;