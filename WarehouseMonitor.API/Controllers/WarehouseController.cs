using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseMonitor.Application.Dtos.Warehouse;
using WarehouseMonitor.Application.Warehouse.Commands;
using WarehouseMonitor.Application.Warehouse.Queries;

namespace WarehouseMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // если хотите защитить
public class WarehouseController : ControllerBase
{
    private readonly IMediator _mediator;
    public WarehouseController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<WarehouseResponseDto>>> GetAll()
    {
        var warehouses = await _mediator.Send(new GetWarehousesQuery());
        return Ok(warehouses);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WarehouseResponseDto>> GetById(Guid id)
    {
        var warehouse = await _mediator.Send(new GetWarehouseByIdQuery(id));
        if (warehouse == null) return NotFound();
        return Ok(warehouse);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateWarehouseCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateWarehouseCommand command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteWarehouseCommand(id));
        return NoContent();
    }
}