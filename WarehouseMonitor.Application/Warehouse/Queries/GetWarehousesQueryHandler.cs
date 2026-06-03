using AutoMapper;
using MediatR;
using WarehouseMonitor.Application.Dtos.Warehouse;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.Warehouse.Queries;

public class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, IReadOnlyList<WarehouseResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWarehousesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<WarehouseResponseDto>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
    {
        var warehouses = await _unitOfWork.Warehouses.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<WarehouseResponseDto>>(warehouses);
    }
}