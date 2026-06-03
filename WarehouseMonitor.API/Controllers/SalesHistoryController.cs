using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseMonitor.Application.Dtos.SalesHistory;
using WarehouseMonitor.Application.SalesHistory.Commands;
using WarehouseMonitor.Application.SalesHistory.Queries;

namespace WarehouseMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesHistoryController : ControllerBase
{
    private readonly IMediator _mediator;
    public SalesHistoryController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<Guid>> AddSalesHistory(AddSalesHistoryCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<ActionResult<IReadOnlyList<SalesHistoryResponseDto>>> GetByProduct(
        Guid productId, 
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate)
    {
        var sales = await _mediator.Send(new GetSalesHistoryForProductQuery(productId, startDate, endDate));
        return Ok(sales);
    }
}