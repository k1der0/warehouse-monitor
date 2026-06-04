using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WarehouseMonitor.Application.Mapping;

namespace WarehouseMonitor.Tests.Mapping;

[TestFixture]
public class MappingProfileTests
{
    [Test]
    public void AutoMapper_Configuration_IsValid()
    {
        var services = new ServiceCollection();
        services.AddAutoMapper(typeof(MappingProfile));
        var serviceProvider = services.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<IMapper>();
        
        // Проверка конфигурации
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}