using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseMonitor.Domain.Entities;

namespace WarehouseMonitor.Infrastructure.Configurations;

public class ForecastConfiguration : IEntityTypeConfiguration<Forecast>
{
    public void Configure(EntityTypeBuilder<Forecast> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.ForecastDate)
            .IsRequired();

        builder.Property(f => f.PredictedDemand)
            .IsRequired();

        builder.Property(f => f.LowerBound)
            .IsRequired();

        builder.Property(f => f.UpperBound)
            .IsRequired();

        builder.Property(f => f.GeneratedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Индексы
        builder.HasIndex(f => f.ForecastDate);
        builder.HasIndex(f => new { f.ProductId, f.ForecastDate });
        builder.HasIndex(f => f.GeneratedAt);

        // Связь с Product
        builder.HasOne(f => f.Product)
            .WithMany(p => p.Forecasts)
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}