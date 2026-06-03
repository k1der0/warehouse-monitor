using System.Text;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WarehouseMonitor.Application;
using WarehouseMonitor.Application.Behaviors;
using WarehouseMonitor.Application.Mapping;
using WarehouseMonitor.Application.Products.Commands.Validators;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Infrastructure.Data;
using WarehouseMonitor.Infrastructure.DependencyInjection;

namespace WarehouseMonitor.API;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1. Добавляем контроллеры и базовые сервисы API
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // 2. Swagger с поддержкой Bearer token (JWT)
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите ваш JWT токен"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // 3. CORS (для React-приложения, работающего на http://localhost:5173)
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // 4. База данных PostgreSQL через Entity Framework Core
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // 5. JWT аутентификация
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
        builder.Services.AddAuthorization();

        // 6. Регистрация репозиториев, UnitOfWork и прочих сервисов Infrastructure
        builder.Services.AddInfrastructure(builder.Configuration);

        // 7. MediatR – сканируем сборку Application (маркер ApplicationAssemblyMarker)
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly));

        // 8. AutoMapper – профиль MappingProfile из Application
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        // 9. FluentValidation + MediatR pipeline behavior для автоматической валидации
        builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommandValidator).Assembly);
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // 10. Hangfire (фоновые задачи) с хранением в PostgreSQL
        builder.Services.AddHangfire(config =>
            config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddHangfireServer();

        var jwtKey = builder.Configuration["Jwt:Key"];
        var jwtIssuer = builder.Configuration["Jwt:Issuer"];
        var jwtAudience = builder.Configuration["Jwt:Audience"];
        Console.WriteLine($"JWT Key length: {jwtKey?.Length ?? 0}");
        Console.WriteLine($"JWT Issuer: {jwtIssuer}");
        Console.WriteLine($"JWT Audience: {jwtAudience}");

        var app = builder.Build();

        // 11. Настройка конвейера middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowReactApp");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.MapControllers();
        app.UseHangfireDashboard("/hangfire"); // доступ к дашборду Hangfire

        // Временный блок для генерации тестовых данных (только для разработки)
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var products = dbContext.Products.ToList();

            foreach (var product in products)
            {
                // Проверяем, есть ли уже история продаж для этого товара
                bool hasSales = dbContext.SalesHistories.Any(sh => sh.ProductId == product.Id);
                if (!hasSales)
                {
                    Console.WriteLine($"Generating sales history for product {product.Name}...");
                    var random = new Random();
                    var startDate = DateTime.UtcNow.AddDays(-90);
                    var baseSales = 10; // средние продажи в день

                    for (int i = 0; i < 90; i++)
                    {
                        var currentDate = startDate.AddDays(i);
                        // Добавляем тренд (медленный рост) и сезонность (например, выходные больше)
                        var trend = i * 0.05; // небольшой рост со временем
                        var dayOfWeek = (int)currentDate.DayOfWeek;
                        var weekendFactor = dayOfWeek == 0 || dayOfWeek == 6 ? 1.3 : 1.0;
                        var quantity = (int)((baseSales + trend) * weekendFactor * (0.7 + random.NextDouble() * 0.6));
                        quantity = Math.Max(1, quantity);

                        var salesHistory = new SalesHistory
                        {
                            Id = Guid.NewGuid(),
                            ProductId = product.Id,
                            SaleDate = currentDate,
                            QuantitySold = quantity,
                            Revenue = quantity * (product.UnitPrice?.Amount ?? 100m)
                        };
                        dbContext.SalesHistories.Add(salesHistory);
                    }
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine($"Generated 90 days of sales for product {product.Name}.");
                }
            }
        } 

        app.Run();
    }
}