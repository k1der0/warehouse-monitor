using AutoMapper;
using MediatR;
using WarehouseMonitor.Application.Dtos.StockMovement;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.StockMovements.Queries;

public class GetMovementsByProductQueryHandler : IRequestHandler<GetMovementsByProductQuery, IReadOnlyList<StockMovementResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetMovementsByProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<StockMovementResponseDto>> Handle(GetMovementsByProductQuery request, CancellationToken cancellationToken)
    {
        var movements = await _unitOfWork.StockMovements
            .FindAsync(m => m.ProductId == request.ProductId, cancellationToken);
        return _mapper.Map<IReadOnlyList<StockMovementResponseDto>>(movements);
    }
}