using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicios.Atracciones.Catalog.Business.DTOs.Attraction;
using Microservicios.Atracciones.Catalog.Business.DTOs.Master;
using Microservicios.Atracciones.Catalog.Business.Interfaces;

namespace Microservicios.Atracciones.Catalog.API.Controllers.V1;

[ApiController]
[Route("api/v1/inclusion")]
public class InclusionController : ControllerBase
{
    private readonly IMasterDataService _masterData;

    public InclusionController(IMasterDataService masterData)
    {
        _masterData = masterData;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<InclusionResponse>>> GetAll()
    {
        var result = await _masterData.GetInclusionsAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<InclusionResponse>> GetById(Guid id)
    {
        var result = await _masterData.GetInclusionByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateInclusionRequest request)
    {
        var id = await _masterData.CreateInclusionAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(Guid id, [FromBody] CreateInclusionRequest request)
    {
        var success = await _masterData.UpdateInclusionAsync(id, request);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var success = await _masterData.DeleteInclusionAsync(id);
        return success ? NoContent() : NotFound();
    }
}


