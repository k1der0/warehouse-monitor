using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseMonitor.Application.Abstractions;
using WarehouseMonitor.Domain.Repositories;
using WarehouseMonitor.Infrastructure.Data;
using WarehouseMonitor.Infrastructure.Repositories;
using WarehouseMonitor.Infrastructure.Services;

namespace WarehouseMonitor.Infrastructure.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<ISalesHistoryRepository, SalesHistoryRepository>();
        services.AddScoped<IForecastRepository, ForecastRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IForecastService, ForecastService>();

        return services;
    }
}