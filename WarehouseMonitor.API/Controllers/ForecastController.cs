using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseMonitor.Application.Dtos.Forecast;
using WarehouseMonitor.Application.Forecast.Commands;
using WarehouseMonitor.Application.Forecast.Queries;

namespace WarehouseMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ForecastController : ControllerBase
{
    private readonly IMediator _mediator;
    public ForecastController(IMediator mediator) => _mediator = mediator;

    [HttpPost("generate/{productId:guid}")]
    public async Task<ActionResult<bool>> GenerateForecast(Guid productId, [FromQuery] int forecastDays = 30)
    {
        var result = await _mediator.Send(new GenerateForecastCommand(productId, forecastDays));
        return Ok(result);
    }

    [HttpGet("latest/{productId:guid}")]
    public async Task<ActionResult<ForecastResponseDto?>> GetLatestForecast(Guid productId)
    {
        var forecast = await _mediator.Send(new GetLatestForecastQuery(productId));
        if (forecast == null) return NotFound();
        return Ok(forecast);
    }
}