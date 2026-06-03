using AutoMapper;
using MediatR;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Domain.ValueObjects;

namespace WarehouseMonitor.Application.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Проверка уникальности SKU
        var existingProduct = await _unitOfWork.Products.GetBySkuAsync(request.Sku, cancellationToken);
        if (existingProduct != null)
            throw new InvalidOperationException($"Product with SKU '{request.Sku}' already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Sku = request.Sku,
            Description = request.Description,
            CurrentStock = request.CurrentStock,
            ReorderPoint = request.ReorderPoint,
            SafetyStock = request.SafetyStock,
            IsActive = true,
            UnitPrice = request.UnitPrice.HasValue ? new Money(request.UnitPrice.Value, "USD") : null
        };

        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}