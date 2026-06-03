using MediatR;
using WarehouseMonitor.Application.Dtos.Warehouse;

namespace WarehouseMonitor.Application.Warehouse.Queries;

public record GetWarehousesQuery() : IRequest<IReadOnlyList<WarehouseResponseDto>>;