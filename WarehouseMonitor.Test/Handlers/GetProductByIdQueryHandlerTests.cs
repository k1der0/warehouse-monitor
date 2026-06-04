using AutoMapper;
using Moq;
using NUnit.Framework;
using WarehouseMonitor.Application.Mapping;
using WarehouseMonitor.Application.Products.Queries;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Domain.ValueObjects;

namespace WarehouseMonitor.Tests.Handlers;

[TestFixture]
public class GetProductByIdQueryHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private GetProductByIdQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Исправленный способ для AutoMapper 12.0.1
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetProductByIdQueryHandler(_unitOfWorkMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_ProductExists_ShouldReturnProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Id = productId,
            Name = "Тестовый товар",
            Sku = "TEST001",
            CurrentStock = 10,
            UnitPrice = new Money(1000, "USD")
        };

        _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var query = new GetProductByIdQuery(productId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(productId));
        Assert.That(result.Name, Is.EqualTo("Тестовый товар"));
    }

    [Test]
    public async Task Handle_ProductNotFound_ShouldReturnNull()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _unitOfWorkMock.Setup(x => x.Products.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var query = new GetProductByIdQuery(productId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Null);
    }
}