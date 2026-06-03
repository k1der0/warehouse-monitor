namespace WarehouseMonitor.Domain.Entities;

public class Warehouse
{
    public Guid Id{ get; set; }
    public string Name { get; set; } =  string.Empty;
    public string Location {get; set;} = string.Empty;
    
    public ICollection<InventoryLevel> InventoryLevels { get; set; } = new List<InventoryLevel>();
}