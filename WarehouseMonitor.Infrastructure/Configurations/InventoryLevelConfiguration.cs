using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Infrastructure.Configurations;

public class InventoryLevelConfiguration : IEntityTypeConfiguration<InventoryLevel>
{
    public void Configure(EntityTypeBuilder<InventoryLevel> builder)
    {
        builder.HasKey(il => il.Id);

        builder.Property(il => il.QuantityOnHand)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(il => il.LastUpdated)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Уникальное ограничение: одна запись на пару (Product, Warehouse)
        builder.HasIndex(il => new { il.ProductId, il.WarehouseId })
            .IsUnique();

        // Связь с Product
        builder.HasOne(il => il.Product)
            .WithMany(p => p.InventoryLevels)
            .HasForeignKey(il => il.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь с Warehouse
        builder.HasOne(il => il.Warehouse)
            .WithMany(w => w.InventoryLevels)
            .HasForeignKey(il => il.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}