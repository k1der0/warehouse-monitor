using MediatR;
using WarehouseMonitor.Application.Dtos.Product;

namespace WarehouseMonitor.Application.Products.Queries;

public record GetProductsQuery() : IRequest<IReadOnlyList<ProductResponseDto>>;