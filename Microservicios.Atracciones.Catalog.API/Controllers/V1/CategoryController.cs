using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microservicios.Atracciones.Catalog.Business.DTOs.Category;
using Microservicios.Atracciones.Catalog.Business.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservicios.Atracciones.Catalog.API.Controllers.V1;

[ApiController]
[Route("api/v1/category")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // â”€â”€â”€ CATEGORÃAS â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll()
    {
        var result = await _categoryService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<CategoryResponse>> GetById(Guid id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var id = await _categoryService.CreateCategoryAsync(request);
        return StatusCode(201, new { id, message = "CategorÃ­a creada con Ã©xito." });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateCategory(Guid id, [FromBody] CreateCategoryRequest request)
    {
        var success = await _categoryService.UpdateCategoryAsync(id, request);
        if (!success) return NotFound();
        return Ok(new { message = "CategorÃ­a actualizada con Ã©xito." });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var success = await _categoryService.DeleteCategoryAsync(id);
        if (!success) return NotFound();
        return Ok(new { message = "CategorÃ­a eliminada con Ã©xito." });
    }

    // â”€â”€â”€ SUBCATEGORÃAS â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [HttpGet("subcategory")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<SubcategoryResponse>>> GetAllSubcategories()
    {
        var all = await _categoryService.GetAllAsync();
        var subcategories = all.SelectMany(c => c.Subcategories);
        return Ok(subcategories);
    }

    [HttpGet("{categoryId:guid}/subcategory")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<SubcategoryResponse>>> GetSubcategoriesByCategory(Guid categoryId)
    {
        var category = await _categoryService.GetByIdAsync(categoryId);
        if (category == null) return NotFound();
        return Ok(category.Subcategories);
    }

    [HttpPost("subcategory")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> CreateSubcategory([FromBody] CreateSubcategoryRequest request)
    {
        var id = await _categoryService.CreateSubcategoryAsync(request);
        return StatusCode(201, new { id, message = "SubcategorÃ­a creada con Ã©xito." });
    }

    [HttpPut("subcategory/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateSubcategory(Guid id, [FromBody] CreateSubcategoryRequest request)
    {
        var success = await _categoryService.UpdateSubcategoryAsync(id, request);
        if (!success) return NotFound();
        return Ok(new { message = "SubcategorÃ­a actualizada con Ã©xito." });
    }

    [HttpDelete("subcategory/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteSubcategory(Guid id)
    {
        var success = await _categoryService.DeleteSubcategoryAsync(id);
        if (!success) return NotFound();
        return Ok(new { message = "SubcategorÃ­a eliminada con Ã©xito." });
    }
}

