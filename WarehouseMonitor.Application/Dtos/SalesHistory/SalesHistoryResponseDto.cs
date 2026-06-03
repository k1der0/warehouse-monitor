namespace WarehouseMonitor.Application.Dtos.SalesHistory;

public class SalesHistoryResponseDto
{
    public DateTime SaleDate { get; set; }
    public int QuantitySold { get; set; }
    public decimal? Revenue { get; set; }
}