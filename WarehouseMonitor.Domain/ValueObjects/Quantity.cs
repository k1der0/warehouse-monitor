namespace WarehouseMonitor.Domain.ValueObjects;

public record Quantity
{
    public int Value { get; init; }
    public string Unit { get; init; } // "pcs", "kg", "liters"
    
    public Quantity(int value, string unit)
    {
        if (value < 0) throw new ArgumentException("Quantity cannot be negative");
        Value = value;
        Unit = unit;
    }
    
    public static Quantity Zero(string unit) => new(0, unit);
    
    public Quantity Add(Quantity other)
    {
        if (Unit != other.Unit)
            throw new InvalidOperationException($"Cannot add different units: {Unit} and {other.Unit}");
        return new Quantity(Value + other.Value, Unit);
    }
}