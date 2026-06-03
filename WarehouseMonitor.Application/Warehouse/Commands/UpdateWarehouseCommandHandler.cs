using MediatR;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.Warehouse.Commands;

public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWarehouseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.Id, cancellationToken);
        if (warehouse == null)
            throw new Exception($"Warehouse with ID {request.Id} not found.");

        warehouse.Name = request.Name;
        warehouse.Location = request.Location;

        _unitOfWork.Warehouses.Update(warehouse);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}