using Moq;
using NUnit.Framework;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.Services;

namespace WarehouseMonitor.Tests.Services;

[TestFixture]
public class ForecastServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private ForecastService _forecastService;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _forecastService = new ForecastService(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task GenerateForecastForProductAsync_NoSalesHistory_ShouldReturnEmptyList()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _unitOfWorkMock.Setup(x => x.SalesHistories.GetSalesForProductAsync(
                productId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SalesHistory>());

        // Act
        var result = await _forecastService.GenerateForecastForProductAsync(productId, 30, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GenerateForecastForProductAsync_InsufficientData_ShouldReturnEmptyList()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var salesHistory = new List<SalesHistory>();
        
        // Только 5 записей (меньше 7)
        for (int i = 0; i < 5; i++)
        {
            salesHistory.Add(new SalesHistory
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                SaleDate = DateTime.UtcNow.AddDays(-i),
                QuantitySold = 10
            });
        }

        _unitOfWorkMock.Setup(x => x.SalesHistories.GetSalesForProductAsync(
                productId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(salesHistory);

        // Act
        var result = await _forecastService.GenerateForecastForProductAsync(productId, 30, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(0));
    }
}