using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Infrastructure.Configurations;

public class SalesHistoryConfiguration : IEntityTypeConfiguration<SalesHistory>
{
    public void Configure(EntityTypeBuilder<SalesHistory> builder)
    {
        builder.HasKey(sh => sh.Id);

        builder.Property(sh => sh.SaleDate)
            .IsRequired();

        builder.Property(sh => sh.QuantitySold)
            .IsRequired();

        builder.Property(sh => sh.Revenue)
            .HasPrecision(18, 2);

        // Индексы
        builder.HasIndex(sh => sh.SaleDate);
        builder.HasIndex(sh => new { sh.ProductId, sh.SaleDate });

        // Связь с Product
        builder.HasOne(sh => sh.Product)
            .WithMany(p => p.SalesHistory)
            .HasForeignKey(sh => sh.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}