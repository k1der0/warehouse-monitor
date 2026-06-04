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
public class GetProductsQueryHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private GetProductsQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetProductsQueryHandler(_unitOfWorkMock.Object, _mapper);
    }

    [Test]
    public async Task Handle_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Товар 1", Sku = "SKU001", CurrentStock = 10, UnitPrice = new Money(1000, "USD") },
            new() { Id = Guid.NewGuid(), Name = "Товар 2", Sku = "SKU002", CurrentStock = 20, UnitPrice = new Money(2000, "USD") },
            new() { Id = Guid.NewGuid(), Name = "Товар 3", Sku = "SKU003", CurrentStock = 30, UnitPrice = new Money(3000, "USD") }
        };

        _unitOfWorkMock.Setup(x => x.Products.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var query = new GetProductsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result[0].Name, Is.EqualTo("Товар 1"));
        Assert.That(result[1].Name, Is.EqualTo("Товар 2"));
        Assert.That(result[2].Name, Is.EqualTo("Товар 3"));
    }

    [Test]
    public async Task Handle_NoProducts_ShouldReturnEmptyList()
    {
        // Arrange
        _unitOfWorkMock.Setup(x => x.Products.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        var query = new GetProductsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
    }
}