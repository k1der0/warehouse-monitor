namespace WarehouseMonitor.Domain.Entities;

public class SalesHistory
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public DateTime SaleDate { get; set; }
    public int QuantitySold { get; set; }
    public decimal? Revenue { get; set; }

    // Навигационное свойство
    public Product Product { get; set; } = null!;
}