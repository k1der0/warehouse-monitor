namespace WarehouseMonitor.Application.Dtos.Other;

public class ValidationErrorDto
{
    public string Property { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}