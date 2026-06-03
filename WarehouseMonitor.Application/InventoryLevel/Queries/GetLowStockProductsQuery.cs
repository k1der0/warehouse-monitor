using MediatR;
using WarehouseMonitor.Application.Dtos.InventoryLevel;

namespace WarehouseMonitor.Application.InventoryLevel.Queries;

public record GetLowStockProductsQuery(int Threshold = 0) : IRequest<IReadOnlyList<LowStockAlertDto>>;
// если Threshold = 0, используется ReorderPoint из продукта