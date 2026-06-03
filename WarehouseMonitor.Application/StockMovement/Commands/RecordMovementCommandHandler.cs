using MediatR;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.StockMovement.Commands;

public class RecordMovementCommandHandler : IRequestHandler<RecordMovementCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public RecordMovementCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Guid> Handle(RecordMovementCommand request, CancellationToken cancellationToken)
{
    var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);
    if (product == null)
        throw new Exception($"Product {request.ProductId} not found");

    // Склад по умолчанию
    var warehouseId = request.WarehouseId ?? Guid.Empty;
    if (warehouseId == Guid.Empty)
    {
        var warehouse = (await _unitOfWork.Warehouses.GetAllAsync(cancellationToken)).FirstOrDefault();
        if (warehouse == null) throw new Exception("No warehouses. Create one first.");
        warehouseId = warehouse.Id;
    }

    // Нормализуем количество в зависимости от типа движения
    int normalizedQuantity = request.MovementType == "Sale" 
        ? -Math.Abs(request.Quantity) 
        : Math.Abs(request.Quantity);

    // Создаём движение
    var movement = new Domain.Entities.StockMovement
    {
        Id = Guid.NewGuid(),
        ProductId = request.ProductId,
        WarehouseId = warehouseId,
        MovementDate = DateTime.UtcNow,
        Quantity = normalizedQuantity,
        MovementType = request.MovementType,
        Reference = request.Reference,
        Note = request.Note
    };

    // InventoryLevel
    var inventory = await _unitOfWork.Inventories
        .GetByProductAndWarehouseAsync(request.ProductId, warehouseId, cancellationToken);
    if (inventory == null)
    {
        inventory = new Domain.Entities.InventoryLevel
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            WarehouseId = warehouseId,
            QuantityOnHand = normalizedQuantity,
            LastUpdated = DateTime.UtcNow
        };
        await _unitOfWork.Inventories.AddAsync(inventory, cancellationToken);
    }
    else
    {
        inventory.QuantityOnHand += normalizedQuantity;
        inventory.LastUpdated = DateTime.UtcNow;
        _unitOfWork.Inventories.Update(inventory);
    }

    // Обновляем CurrentStock товара
    product.CurrentStock += normalizedQuantity;
    _unitOfWork.Products.Update(product);

    // Если продажа, добавляем запись в SalesHistory (с положительным количеством)
    if (request.MovementType == "Sale" && normalizedQuantity < 0)
    {
        var salesHistory = new Domain.Entities.SalesHistory
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            SaleDate = DateTime.UtcNow,
            QuantitySold = Math.Abs(normalizedQuantity),
            Revenue = Math.Abs(normalizedQuantity) * (product.UnitPrice?.Amount ?? 0)
        };
        await _unitOfWork.SalesHistories.AddAsync(salesHistory, cancellationToken);
    }

    await _unitOfWork.StockMovements.AddAsync(movement, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return movement.Id;
}
}