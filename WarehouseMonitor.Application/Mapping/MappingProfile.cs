using AutoMapper;
using WarehouseMonitor.Application.Dtos.Forecast;
using WarehouseMonitor.Application.Dtos.InventoryLevel;
using WarehouseMonitor.Application.Dtos.Product;
using WarehouseMonitor.Application.Dtos.SalesHistory;
using WarehouseMonitor.Application.Dtos.StockMovement;
using WarehouseMonitor.Application.Dtos.Warehouse;
using WarehouseMonitor.Domain.Entities;
using WarehouseMonitor.Domain.ValueObjects;

namespace WarehouseMonitor.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ==================== Глобальные преобразования ====================
        // Money -> decimal? (только Amount)
        CreateMap<Money, decimal?>().ConvertUsing(src => src.Amount);
        // decimal? -> Money (с валютой по умолчанию, например "USD")
        CreateMap<decimal?, Money>().ConvertUsing(src => src.HasValue ? new Money(src.Value, "USD") : null);

        // ==================== Product ====================
        // Product -> ProductResponseDto
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NeedsReorder,
                opt => opt.MapFrom(src => src.CurrentStock <= src.ReorderPoint))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

        // ProductResponseDto -> Product (обратный)
        CreateMap<ProductResponseDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.StockMovements, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryLevels, opt => opt.Ignore())
            .ForMember(dest => dest.SalesHistory, opt => opt.Ignore())
            .ForMember(dest => dest.Forecasts, opt => opt.Ignore());

        // Product -> ProductBriefResponseDto
        CreateMap<Product, ProductBriefResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        // CreateProductRequestDto -> Product
        CreateMap<CreateProductRequestDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.StockMovements, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryLevels, opt => opt.Ignore())
            .ForMember(dest => dest.SalesHistory, opt => opt.Ignore())
            .ForMember(dest => dest.Forecasts, opt => opt.Ignore());

        // UpdateProductRequestDto -> Product
        CreateMap<UpdateProductRequestDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // если в DTO свойство Id
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.StockMovements, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryLevels, opt => opt.Ignore())
            .ForMember(dest => dest.SalesHistory, opt => opt.Ignore())
            .ForMember(dest => dest.Forecasts, opt => opt.Ignore());

        // ==================== StockMovement ====================
        CreateMap<Domain.Entities.StockMovement, StockMovementResponseDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductSku,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Sku : string.Empty));

        CreateMap<RecordMovementRequestDto, Domain.Entities.StockMovement>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MovementDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Warehouse, opt => opt.Ignore());

        // ==================== Inventory ====================
        CreateMap<Domain.Entities.InventoryLevel, InventoryLevelResponseDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductSku,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Sku : string.Empty))
            .ForMember(dest => dest.WarehouseName,
                opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : string.Empty));

        CreateMap<Domain.Entities.InventoryLevel, LowStockAlertDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.Sku,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Sku : string.Empty))
            .ForMember(dest => dest.CurrentStock, opt => opt.MapFrom(src => src.QuantityOnHand))
            .ForMember(dest => dest.ReorderPoint,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.ReorderPoint : 0));

        // ==================== SalesHistory ====================
        CreateMap<Domain.Entities.SalesHistory, SalesHistoryResponseDto>();
        CreateMap<SalesHistoryRecordDto, Domain.Entities.SalesHistory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore());

        // ==================== Forecast ====================
        CreateMap<Domain.Entities.Forecast, ForecastResponseDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));

        // ==================== Warehouse ====================
        CreateMap<Domain.Entities.Warehouse, WarehouseResponseDto>();
        CreateMap<CreateWarehouseRequestDto, Domain.Entities.Warehouse>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.InventoryLevels, opt => opt.Ignore());
    }
}