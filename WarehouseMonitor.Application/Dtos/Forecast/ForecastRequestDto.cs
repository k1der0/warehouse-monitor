namespace WarehouseMonitor.Application.Dtos.Forecast;

public class ForecastRequestDto
{
    public Guid ProductId { get; set; }
    public int ForecastDays { get; set; } = 30;          // на сколько дней вперёд
}