using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicios.Atracciones.Catalog.Business.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservicios.Atracciones.Catalog.API.Controllers.V1;

[ApiController]
[Route("api/v1/location")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LocationNode>>> GetHierarchy()
    {
        var result = await _locationService.GetHierarchyAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<LocationNode>> GetById(Guid id)
    {
        var result = await _locationService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateLocationRequest request)
    {
        var id = await _locationService.CreateAsync(request);
        return StatusCode(201, new { id, message = "UbicaciÃ³n creada con Ã©xito." });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(Guid id, [FromBody] CreateLocationRequest request)
    {
        var success = await _locationService.UpdateAsync(id, request);
        if (!success) return NotFound();
        return Ok(new { message = "UbicaciÃ³n actualizada con Ã©xito." });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var success = await _locationService.DeleteAsync(id);
        if (!success) return NotFound();
        return Ok(new { message = "UbicaciÃ³n eliminada con Ã©xito." });
    }
}

