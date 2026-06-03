using AutoMapper;
using MediatR;
using WarehouseMonitor.Application.Dtos.SalesHistory;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.SalesHistory.Queries;

public class GetSalesHistoryForProductQueryHandler : IRequestHandler<GetSalesHistoryForProductQuery, IReadOnlyList<SalesHistoryResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetSalesHistoryForProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<SalesHistoryResponseDto>> Handle(GetSalesHistoryForProductQuery request, CancellationToken cancellationToken)
    {
        var sales = await _unitOfWork.SalesHistories
            .GetSalesForProductAsync(request.ProductId, request.StartDate, request.EndDate, cancellationToken);
        return _mapper.Map<IReadOnlyList<SalesHistoryResponseDto>>(sales);
    }
}