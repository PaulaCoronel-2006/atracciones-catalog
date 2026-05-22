using System.ComponentModel.DataAnnotations;

namespace Microservicios.Atracciones.Catalog.Business.DTOs.Category;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public short SortOrder { get; set; }
    public bool IsActive { get; set; }
    public List<SubcategoryResponse> Subcategories { get; set; } = [];
}

public class CreateCategoryRequest
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    public string Name { get; set; } = string.Empty;
    
    public string? IconUrl { get; set; }
    public short SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

public class SubcategoryResponse
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
}

public class CreateSubcategoryRequest
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La categorÃ­a base es requerida.")]
    public Guid CategoryId { get; set; }
    
    public string? IconUrl { get; set; }
    public short SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

