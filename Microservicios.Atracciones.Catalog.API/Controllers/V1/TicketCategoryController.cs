using Microsoft.AspNetCore.Mvc;
using Microservicios.Atracciones.Catalog.Business.DTOs.Common;
using Microservicios.Atracciones.Catalog.Business.DTOs.Master;
using Microservicios.Atracciones.Catalog.Business.Interfaces;

namespace Microservicios.Atracciones.Catalog.API.Controllers.V1;

[ApiController]
[Route("api/v1/ticketcategory")]
public class TicketCategoryController : ControllerBase
{
    private readonly IMasterDataService _masterData;

    public TicketCategoryController(IMasterDataService masterData)
    {
        _masterData = masterData;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketCategoryResponse>>> GetAll()
    {
        var result = await _masterData.GetTicketCategoriesAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TicketCategoryResponse>> GetById(Guid id)
    {
        var result = await _masterData.GetTicketCategoryByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateTicketCategoryRequest request)
    {
        var id = await _masterData.CreateTicketCategoryAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] CreateTicketCategoryRequest request)
    {
        var success = await _masterData.UpdateTicketCategoryAsync(id, request);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var success = await _masterData.DeleteTicketCategoryAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}

