using MediatR;

namespace WarehouseMonitor.Application.Products.Commands;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Sku,
    string? Description,
    int CurrentStock,
    int ReorderPoint,
    int SafetyStock,
    bool IsActive,
    decimal? UnitPrice = null
) : IRequest;