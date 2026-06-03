using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseMonitor.Application.Dtos.StockMovement;
using WarehouseMonitor.Application.StockMovement.Commands;
using WarehouseMonitor.Application.StockMovements.Queries;

namespace WarehouseMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StockMovementsController : ControllerBase
{
    private readonly IMediator _mediator;
    public StockMovementsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<Guid>> RecordMovement(RecordMovementCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<ActionResult<IReadOnlyList<StockMovementResponseDto>>> GetByProduct(Guid productId)
    {
        var movements = await _mediator.Send(new GetMovementsByProductQuery(productId));
        return Ok(movements);
    }
}