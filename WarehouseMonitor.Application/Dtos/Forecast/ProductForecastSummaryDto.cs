namespace WarehouseMonitor.Application.Dtos.Forecast;

public class ProductForecastSummaryDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int TotalPredictedDemandNext30Days { get; set; }
    public bool NeedsReorder => CurrentStock < TotalPredictedDemandNext30Days;
}