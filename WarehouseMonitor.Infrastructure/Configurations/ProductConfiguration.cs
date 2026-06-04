using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Первичный ключ
        builder.HasKey(p => p.Id);

        // Свойства
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);

        builder.Property(p => p.Sku).IsRequired().HasMaxLength(50);

        builder.HasIndex(p => p.Sku).IsUnique();

        builder.Property(p => p.Description).HasMaxLength(1000);

        builder.Property(p => p.CurrentStock).HasDefaultValue(0);

        builder.Property(p => p.ReorderPoint).HasDefaultValue(5);

        builder.Property(p => p.SafetyStock).HasDefaultValue(2);

        builder.Property(p => p.IsActive).HasDefaultValue(true);

        // Конфигурация Value Object Money как owned type
        builder.OwnsOne(p => p.UnitPrice, money =>
        {
            money.Property(m => m.Amount).HasColumnName("UnitPrice_Amount")
                .HasPrecision(18, 2);

            money.Property(m => m.Currency).HasColumnName("UnitPrice_Currency")
                .HasMaxLength(3).HasDefaultValue("USD");
        });

        // Индексы для часто используемых полей
        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.IsActive);
    }
}