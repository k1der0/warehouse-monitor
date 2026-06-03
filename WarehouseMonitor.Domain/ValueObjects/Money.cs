using Microsoft.EntityFrameworkCore;

namespace WarehouseMonitor.Domain.ValueObjects;

[Owned]
public record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    
    public Money(decimal amount, string currency)
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative");
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required");
        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }
    
    public static Money Zero(string currency = "USD") => new(0, currency);
    
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot add different currencies: {Currency} and {other.Currency}");
        return new Money(Amount + other.Amount, Currency);
    }
    
    // Другие операции по необходимости
}