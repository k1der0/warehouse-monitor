using MediatR;

namespace WarehouseMonitor.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Sku,
    string? Description,
    int CurrentStock,
    int ReorderPoint,
    int SafetyStock,
    decimal? UnitPrice = null
) : IRequest<Guid>;