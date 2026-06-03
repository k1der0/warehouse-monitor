using MediatR;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.Warehouse.Commands;

public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWarehouseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await _unitOfWork.Warehouses.GetByIdAsync(request.Id, cancellationToken);
        if (warehouse == null)
            throw new Exception($"Warehouse with ID {request.Id} not found.");

        _unitOfWork.Warehouses.Delete(warehouse);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}