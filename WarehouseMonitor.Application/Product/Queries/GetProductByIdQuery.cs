using MediatR;
using WarehouseMonitor.Application.Dtos.Product;

namespace WarehouseMonitor.Application.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponseDto?>;