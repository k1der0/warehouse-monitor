namespace WarehouseMonitor.Domain.Entities;

public class Forecast
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public DateTime ForecastDate { get; set; }      // дата, на которую сделан прогноз
    public int PredictedDemand { get; set; }        // прогнозируемый спрос
    public int LowerBound { get; set; }             // нижняя граница (опционально)
    public int UpperBound { get; set; }             // верхняя граница
    public DateTime GeneratedAt { get; set; }       // когда прогноз был создан
    
    public Product Product { get; set; } = null!;
}