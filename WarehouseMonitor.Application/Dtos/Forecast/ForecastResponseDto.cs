namespace WarehouseMonitor.Application.Dtos.Forecast;

public class ForecastResponseDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public DateTime ForecastDate { get; set; }           // день прогноза
    public int PredictedDemand { get; set; }
    public int LowerBound { get; set; }                  // нижняя граница (опционально)
    public int UpperBound { get; set; }                  // верхняя граница
    public DateTime GeneratedAt { get; set; }
}