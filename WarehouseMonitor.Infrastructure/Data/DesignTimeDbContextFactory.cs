using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WarehouseMonitor.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Здесь нужно явно указать строку подключения для миграций.
        // ВАЖНО: Используйте ТУ ЖЕ САМУЮ строку, что и в appsettings.json!
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=warehouse_db;Username=postgres;Password=postgres123");

        return new AppDbContext(optionsBuilder.Options);
    }
}