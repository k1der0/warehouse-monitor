using Moq;
using NUnit.Framework;
using WarehouseMonitor.Application.Products.Commands;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Tests.Handlers;

[TestFixture]
public class DeleteProductCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private DeleteProductCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteProductCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldDeleteProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId, Name = "Товар для удаления" };

        var command = new DeleteProductCommand(productId);

        _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Products.Delete(product), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_ProductNotFound_ShouldThrowException()
    {
        // Arrange
        var command = new DeleteProductCommand(Guid.NewGuid());

        _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => 
            await _handler.Handle(command, CancellationToken.None));
        
        Assert.That(ex.Message, Does.Contain("not found"));
    }
}