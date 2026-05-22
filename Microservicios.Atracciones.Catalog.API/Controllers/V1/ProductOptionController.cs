using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicios.Atracciones.Catalog.Business.DTOs.Inventory;
using Microservicios.Atracciones.Catalog.Business.Interfaces;

namespace Microservicios.Atracciones.Catalog.API.Controllers.V1;

[ApiController]
[Route("api/v1/productoption")]
[Authorize(Roles = "Admin,Partner")]
public class ProductOptionController : ControllerBase
{
    private readonly IProductOptionService _productOptionService;

    public ProductOptionController(IProductOptionService productOptionService)
    {
        _productOptionService = productOptionService;
    }

    [HttpGet("by-attraction/{attractionId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ProductOptionDetailResponse>>> GetByAttraction(Guid attractionId)
    {
        var result = await _productOptionService.GetByAttractionAsync(attractionId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductOptionDetailResponse>> GetById(Guid id)
    {
        var result = await _productOptionService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreateProductOptionRequest request)
    {
        var id = await _productOptionService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, new { id, message = "Modalidad creada con Ã©xito." });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateProductOptionRequest request)
    {
        await _productOptionService.UpdateAsync(id, request);
        return Ok(new { message = "Modalidad actualizada con Ã©xito." });
    }

    [HttpPatch("{id:guid}/toggle")]
    public async Task<ActionResult> Toggle(Guid id, [FromBody] bool isActive)
    {
        var result = await _productOptionService.ToggleActiveAsync(id, isActive);
        if (!result) return NotFound();
        var estado = isActive ? "activada" : "desactivada";
        return Ok(new { message = $"Modalidad {estado}." });
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _productOptionService.DeleteAsync(id);
        if (!result) return NotFound();
        return Ok(new { message = "Modalidad eliminada." });
    }
}

