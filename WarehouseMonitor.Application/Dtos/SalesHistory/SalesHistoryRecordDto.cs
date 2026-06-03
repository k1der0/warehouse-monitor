namespace WarehouseMonitor.Application.Dtos.SalesHistory;

public class SalesHistoryRecordDto
{
    public Guid ProductId { get; set; }
    public DateTime SaleDate { get; set; }
    public int QuantitySold { get; set; }
    public decimal? Revenue { get; set; }
}