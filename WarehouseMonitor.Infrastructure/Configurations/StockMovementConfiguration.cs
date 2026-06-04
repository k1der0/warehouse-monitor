using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Infrastructure.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.HasKey(sm => sm.Id);

        builder.Property(sm => sm.Quantity)
            .IsRequired();

        builder.Property(sm => sm.MovementDate)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(sm => sm.MovementType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(sm => sm.Reference)
            .HasMaxLength(100);

        builder.Property(sm => sm.Note)
            .HasMaxLength(500);

        // Индексы
        builder.HasIndex(sm => sm.MovementDate);
        builder.HasIndex(sm => sm.MovementType);

        // Связь с Product (один товар имеет много движений)
        builder.HasOne(sm => sm.Product)
            .WithMany(p => p.StockMovements)
            .HasForeignKey(sm => sm.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь с Warehouse (один склад имеет много движений)
        builder.HasOne(sm => sm.Warehouse)
            .WithMany(w => w.StockMovements)
            .HasForeignKey(sm => sm.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}