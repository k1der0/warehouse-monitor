using AutoMapper;
using MediatR;
using WarehouseMonitor.Application.Dtos.Forecast;
using WarehouseMonitor.Domain.Repositories;

namespace WarehouseMonitor.Application.Forecast.Queries;

public class GetLatestForecastQueryHandler : IRequestHandler<GetLatestForecastQuery, ForecastResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLatestForecastQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ForecastResponseDto?> Handle(GetLatestForecastQuery request, CancellationToken cancellationToken)
    {
        var forecast = await _unitOfWork.Forecasts.GetLatestForProductAsync(request.ProductId, cancellationToken);
        return forecast == null ? null : _mapper.Map<ForecastResponseDto>(forecast);
    }
}