using FluentValidation.TestHelper;
using NUnit.Framework;
using WarehouseMonitor.Application.Products.Commands;
using WarehouseMonitor.Application.Products.Commands.Validators;

namespace WarehouseMonitor.Tests.Validators;

[TestFixture]
public class CreateProductCommandValidatorTests
{
    private CreateProductCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Test]
    public void Validate_ValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new CreateProductCommand(
            "Ноутбук",
            "NB001",
            "Игровой ноутбук",
            10,
            5,
            2,
            50000);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_EmptyName_ShouldHaveError()
    {
        var command = new CreateProductCommand("", "NB001", null, 10, 5, 2, 50000);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validate_NameTooLong_ShouldHaveError()
    {
        var longName = new string('A', 201);
        var command = new CreateProductCommand(longName, "NB001", null, 10, 5, 2, 50000);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validate_EmptySku_ShouldHaveError()
    {
        var command = new CreateProductCommand("Ноутбук", "", null, 10, 5, 2, 50000);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Sku);
    }

    [Test]
    public void Validate_NegativeCurrentStock_ShouldHaveError()
    {
        var command = new CreateProductCommand("Ноутбук", "NB001", null, -5, 5, 2, 50000);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CurrentStock);
    }

    [Test]
    public void Validate_NegativeUnitPrice_ShouldHaveError()
    {
        var command = new CreateProductCommand("Ноутбук", "NB001", null, 10, 5, 2, -100);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UnitPrice);
    }
}