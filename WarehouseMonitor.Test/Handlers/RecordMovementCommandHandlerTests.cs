using Moq;
using NUnit.Framework;
using WarehouseMonitor.Application.StockMovement.Commands;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Domain.ValueObjects;

namespace WarehouseMonitor.Tests.Handlers;

[TestFixture]
public class RecordMovementCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private RecordMovementCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new RecordMovementCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ValidReceipt_ShouldIncreaseStock()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId, CurrentStock = 10 };
        var warehouseId = Guid.NewGuid();
        var warehouse = new Warehouse { Id = warehouseId, Name = "Склад" };

        var command = new RecordMovementCommand(
            productId,
            5,
            "Receipt",
            "Накладная №123",
            null,
            warehouseId);

        _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _unitOfWorkMock.Setup(x => x.Warehouses.GetByIdAsync(warehouseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(warehouse);

        _unitOfWorkMock.Setup(x => x.Inventories.GetByProductAndWarehouseAsync(
            productId, warehouseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((InventoryLevel?)null);

        _unitOfWorkMock.Setup(x => x.StockMovements.AddAsync(It.IsAny<StockMovement>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        Assert.That(product.CurrentStock, Is.EqualTo(15));
        _unitOfWorkMock.Verify(x => x.StockMovements.AddAsync(It.IsAny<StockMovement>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_ValidSale_ShouldDecreaseStockAndAddSalesHistory()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            CurrentStock = 10,
            UnitPrice = new Money(1000, "USD")
        };
        var warehouseId = Guid.NewGuid();
        var warehouse = new Warehouse { Id = warehouseId, Name = "Склад" };

        var command = new RecordMovementCommand(
            productId,
            3,
            "Sale",
            "Чек №456",
            null,
            warehouseId);

        _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _unitOfWorkMock.Setup(x => x.Warehouses.GetByIdAsync(warehouseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(warehouse);

        _unitOfWorkMock.Setup(x => x.Inventories.GetByProductAndWarehouseAsync(
            productId, warehouseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((InventoryLevel?)null);

        _unitOfWorkMock.Setup(x => x.StockMovements.AddAsync(It.IsAny<StockMovement>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.SalesHistories.AddAsync(It.IsAny<SalesHistory>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(product.CurrentStock, Is.EqualTo(7));
        _unitOfWorkMock.Verify(x => x.SalesHistories.AddAsync(It.IsAny<SalesHistory>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_ProductNotFound_ShouldThrowException()
    {
        // Arrange
        var command = new RecordMovementCommand(Guid.NewGuid(), 5, "Receipt", null, null, null);

        _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Does.Contain("not found"));
    }
}