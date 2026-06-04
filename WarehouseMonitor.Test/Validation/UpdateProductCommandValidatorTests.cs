using FluentValidation.TestHelper;
using NUnit.Framework;
using WarehouseMonitor.Application.Products.Commands;
using WarehouseMonitor.Application.Products.Commands.Validators;

namespace WarehouseMonitor.Tests.Validators;

[TestFixture]
public class UpdateProductCommandValidatorTests
{
    private UpdateProductCommandValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateProductCommandValidator();
    }

    [Test]
    public void Validate_ValidCommand_ShouldNotHaveErrors()
    {
        var command = new UpdateProductCommand(
            Guid.NewGuid(),
            "Ноутбук",
            "NB001",
            "Описание",
            10,
            5,
            2,
            true,
            50000);

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_EmptyId_ShouldHaveError()
    {
        var command = new UpdateProductCommand(
            Guid.Empty,
            "Ноутбук",
            "NB001",
            null,
            10,
            5,
            2,
            true,
            50000);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Validate_EmptyName_ShouldHaveError()
    {
        var command = new UpdateProductCommand(
            Guid.NewGuid(),
            "",
            "NB001",
            null,
            10,
            5,
            2,
            true,
            50000);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validate_EmptySku_ShouldHaveError()
    {
        var command = new UpdateProductCommand(
            Guid.NewGuid(),
            "Ноутбук",
            "",
            null,
            10,
            5,
            2,
            true,
            50000);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Sku);
    }

    [Test]
    public void Validate_NegativeCurrentStock_ShouldHaveError()
    {
        var command = new UpdateProductCommand(
            Guid.NewGuid(),
            "Ноутбук",
            "NB001",
            null,
            -5,
            5,
            2,
            true,
            50000);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CurrentStock);
    }
}