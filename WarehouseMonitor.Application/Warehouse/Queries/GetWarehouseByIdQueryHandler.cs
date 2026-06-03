using AutoMapper;
using MediatR;
using WarehouseMonitor.Application.Dtos.Warehouse;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.Warehouse.Queries;

public class GetWarehouseByIdQueryHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWarehouseByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WarehouseResponseDto?> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.Id, cancellationToken);
        return warehouse == null ? null : _mapper.Map<WarehouseResponseDto>(warehouse);
    }
}