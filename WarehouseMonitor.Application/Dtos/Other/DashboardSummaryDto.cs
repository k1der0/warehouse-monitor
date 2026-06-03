using WarehouseMonitor.Application.Dtos.InventoryLevel;

namespace WarehouseMonitor.Application.Dtos.Other;

public class DashboardSummaryDto
{
    public int TotalProducts { get; set; }
    public int LowStockProductsCount { get; set; }
    public int OutOfStockProductsCount { get; set; }
    public int TotalStockValue { get; set; }        // суммарная стоимость запасов
    public IEnumerable<LowStockAlertDto> CriticalAlerts { get; set; } = new List<LowStockAlertDto>();
}