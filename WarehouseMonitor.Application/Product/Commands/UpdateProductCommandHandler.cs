using AutoMapper;
using MediatR;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Domain.ValueObjects;

namespace WarehouseMonitor.Application.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            throw new Exception($"Product with ID {request.Id} not found.");

        // Проверка уникальности SKU (если изменился)
        if (product.Sku != request.Sku)
        {
            var existing = await _unitOfWork.Products.GetBySkuAsync(request.Sku, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException($"Product with SKU '{request.Sku}' already exists.");
        }

        product.Name = request.Name;
        product.Sku = request.Sku;
        product.Description = request.Description;
        product.CurrentStock = request.CurrentStock;
        product.ReorderPoint = request.ReorderPoint;
        product.SafetyStock = request.SafetyStock;
        product.IsActive = request.IsActive;
        product.UnitPrice = request.UnitPrice.HasValue ? new Money(request.UnitPrice.Value, "USD") : null;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}