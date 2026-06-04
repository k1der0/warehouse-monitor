namespace WarehouseMonitor.Domain.Entities;

public class Forecast
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public DateTime ForecastDate { get; set; }
    public int PredictedDemand { get; set; }
    public int LowerBound { get; set; }
    public int UpperBound { get; set; }
    public DateTime GeneratedAt { get; set; }

    // Навигационное свойство
    public Product Product { get; set; } = null!;
}