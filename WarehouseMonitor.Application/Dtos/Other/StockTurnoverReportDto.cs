namespace WarehouseMonitor.Application.Dtos.Other;

public class StockTurnoverReportDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int TotalSold { get; set; }
    public decimal AverageDailySales { get; set; }
    public int CurrentStock { get; set; }
    public int DaysUntilOutOfStock => CurrentStock == 0 ? 0 : (int)(CurrentStock / AverageDailySales);
}