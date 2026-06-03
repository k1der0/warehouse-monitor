using AutoMapper;
using MediatR;
using WarehouseMonitor.Application.Dtos.Product;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.Products.Queries;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProductResponseDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ProductResponseDto>>(products);
    }
}