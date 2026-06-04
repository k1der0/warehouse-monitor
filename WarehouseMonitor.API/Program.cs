using System.Text;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ML;
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

        app.UseHangfireDashboard("/hangfire"); // ← ПЕРЕМЕСТИТЕ СЮДА

        app.MapControllers();



        // Временная проверка (удалить потом)
        try
        {
            var mlContext = new MLContext();
            Console.WriteLine("ML.NET initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ML.NET initialization failed: {ex.Message}");
        }
        app.Run();
    }
}