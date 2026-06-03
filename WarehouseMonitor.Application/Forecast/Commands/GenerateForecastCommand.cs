using MediatR;

namespace WarehouseMonitor.Application.Forecast.Commands;

public record GenerateForecastCommand(Guid ProductId, int ForecastDays = 30) : IRequest<bool>;