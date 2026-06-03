using MediatR;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.Warehouse.Commands;

public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateWarehouseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = new Domain.Entities.Warehouse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Location = request.Location
        };

        await _unitOfWork.Warehouses.AddAsync(warehouse, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return warehouse.Id;
    }
}