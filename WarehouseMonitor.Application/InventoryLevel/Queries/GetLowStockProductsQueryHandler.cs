using AutoMapper;
using MediatR;
using WarehouseMonitor.Application.Dtos.InventoryLevel;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.InventoryLevel.Queries;

public class GetLowStockProductsQueryHandler : IRequestHandler<GetLowStockProductsQuery, IReadOnlyList<LowStockAlertDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetLowStockProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<LowStockAlertDto>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
    {
        // Получаем все инвентаризации, где остаток <= порог (если порог указан) или <= ReorderPoint продукта
        var inventories = await _unitOfWork.Inventories.GetAllAsync(cancellationToken);
        var lowStock = new List<LowStockAlertDto>();
        
        foreach (var inv in inventories)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(inv.ProductId, cancellationToken);
            if (product == null) continue;
            
            var threshold = request.Threshold > 0 ? request.Threshold : product.ReorderPoint;
            if (inv.QuantityOnHand <= threshold)
            {
                var alert = _mapper.Map<LowStockAlertDto>(inv);
                lowStock.Add(alert);
            }
        }
        return lowStock;
    }
}