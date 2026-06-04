using AutoMapper;
using Moq;
using NUnit.Framework;
using WarehouseMonitor.Application.Mapping;
using WarehouseMonitor.Application.Products.Commands;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Tests.Handlers;

[TestFixture]
public class CreateProductCommandHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private CreateProductCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new CreateProductCommandHandler(_unitOfWorkMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldCreateProduct()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Тестовый товар",
            "TEST001",
            "Описание",
            10,
            5,
            2,
            1000);

        _unitOfWorkMock
            .Setup(x => x.Products.GetBySkuAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        _unitOfWorkMock
            .Setup(x => x.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
        _unitOfWorkMock.Verify(x => x.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void Handle_DuplicateSku_ShouldThrowException()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Тестовый товар",
            "DUPLICATE_SKU",
            null,
            10,
            5,
            2,
            1000);

        var existingProduct = new Product { Id = Guid.NewGuid(), Sku = "DUPLICATE_SKU" };

        _unitOfWorkMock
            .Setup(x => x.Products.GetBySkuAsync("DUPLICATE_SKU", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Does.Contain("already exists"));
    }
}