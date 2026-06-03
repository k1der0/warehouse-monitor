using MediatR;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.SalesHistory.Commands;

public class AddSalesHistoryCommandHandler : IRequestHandler<AddSalesHistoryCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AddSalesHistoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Guid> Handle(AddSalesHistoryCommand request, CancellationToken cancellationToken)
    {
        var sales = new Domain.Entities.SalesHistory
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            SaleDate = request.SaleDate,
            QuantitySold = request.QuantitySold,
            Revenue = request.Revenue
        };
        await _unitOfWork.SalesHistories.AddAsync(sales, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return sales.Id;
    }
}