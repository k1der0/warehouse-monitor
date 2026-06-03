using MediatR;
using WarehouseMonitor.Application.Dtos.Forecast;

namespace WarehouseMonitor.Application.Forecast.Queries;

public record GetLatestForecastQuery(Guid ProductId) : IRequest<ForecastResponseDto?>;