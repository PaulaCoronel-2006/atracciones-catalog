using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicios.Atracciones.Catalog.Business.DTOs.Attraction;
using Microservicios.Atracciones.Catalog.Business.Interfaces;
using Microservicios.Atracciones.Catalog.DataAccess.Common;
using System.Security.Claims;

namespace Microservicios.Atracciones.Catalog.API.Controllers.V1;

[ApiController]
[Route("api/v1/attraction")]
public class AttractionController : ControllerBase
{
    private readonly IAttractionService _attractionService;

    public AttractionController(IAttractionService attractionService)
    {
        _attractionService = attractionService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<AttractionSummaryResponse>>> Search([FromQuery] AttractionSearchRequest request)
    {
        var result = await _attractionService.SearchAsync(request);
        return Ok(result);
    }

    [HttpGet("management")]
    [Authorize(Roles = "Admin,Partner")]
    public async Task<ActionResult<PagedResult<AttractionSummaryResponse>>> SearchManagement([FromQuery] AttractionSearchRequest request)
    {
        var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        bool isAdmin = User.IsInRole("Admin");

        var result = await _attractionService.SearchManagementAsync(request, currentUserId, isAdmin);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Partner")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateAttractionRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        bool isAdmin = User.IsInRole("Admin");
        var id = await _attractionService.CreateAsync(request, userId, isAdmin);
        return CreatedAtAction(nameof(Search), new { id }, id);
    }

    [HttpPost("complete")]
    [Authorize(Roles = "Admin,Partner")]
    public async Task<ActionResult<Guid>> CreateComplete([FromBody] CreateCompleteAttractionRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        bool isAdmin = User.IsInRole("Admin");
        var id = await _attractionService.CreateCompleteAsync(request, userId, isAdmin);
        return Ok(new { id, message = "AtracciÃ³n completa creada con Ã©xito." });
    }

    [HttpGet("top")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AttractionSummaryResponse>>> GetTopRated([FromQuery] int count = 5)
    {
        var result = await _attractionService.GetTopRatedAsync(count);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<AttractionDetailResponse>> GetBySlug(string slug, [FromQuery] short requestedLangId = 1)
    {
        var result = await _attractionService.GetDetailBySlugAsync(slug, requestedLangId);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Partner")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateAttractionRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        bool isAdmin = User.IsInRole("Admin");
        var success = await _attractionService.UpdateAsync(id, request, userId, isAdmin);
        if (!success) return NotFound();
        return Ok(new { message = "AtracciÃ³n actualizada con Ã©xito." });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var success = await _attractionService.DeleteAsync(id, userId, isAdmin: true);
        if (!success) return NotFound();
        return Ok(new { message = "AtracciÃ³n eliminada con Ã©xito." });
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Partner")]
    public async Task<ActionResult> ToggleStatus(Guid id, [FromBody] ToggleAttractionStatusRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        bool isAdmin = User.IsInRole("Admin");
        var success = await _attractionService.ToggleStatusAsync(id, request.IsPublished, userId, isAdmin);
        if (!success) return NotFound();
        var estado = request.IsPublished ? "publicada" : "despublicada";
        return Ok(new { message = $"AtracciÃ³n {estado} con Ã©xito." });
    }

    [HttpPatch("{id:guid}/active")]
    [Authorize(Roles = "Admin,Partner")]
    public async Task<ActionResult> ToggleActive(Guid id, [FromBody] Microservicios.Atracciones.Catalog.Business.DTOs.Attraction.ToggleAttractionActiveRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        bool isAdmin = User.IsInRole("Admin");
        
        try
        {
            var success = await _attractionService.ToggleActiveAsync(id, request.IsActive, userId, isAdmin);
            if (!success) return NotFound();
            
            var estado = request.IsActive ? "activada" : "desactivada";
            return Ok(new { message = $"AtracciÃ³n {estado} con Ã©xito." });
        }
        catch (Business.Exceptions.ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/complete")]
    [Authorize(Roles = "Admin,Partner")]
    public async Task<ActionResult<Microservicios.Atracciones.Catalog.Business.DTOs.Attraction.AttractionFullEditionResponse>> GetCompleteDetail(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        bool isAdmin = User.IsInRole("Admin");

        var result = await _attractionService.GetCompleteByIdAsync(id, userId, isAdmin);
        if (result == null) return NotFound(new { message = "La atracciÃ³n no existe o fue eliminada." });

        return Ok(result);
    }
}

