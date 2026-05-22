using Microservicios.Atracciones.Catalog.Business.DTOs.Category;
using Microservicios.Atracciones.Catalog.Business.Interfaces;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Microservicios.Atracciones.Catalog.Business.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryDataService _categoryData;

    public CategoryService(ICategoryDataService categoryData)
    {
        _categoryData = categoryData;
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
    {
        var entities = await _categoryData.GetAllWithSubcategoriesAsync();
        return entities.Select(MapToResponse);
    }

    public async Task<CategoryResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _categoryData.GetByIdAsync(id);
        return entity == null ? null : MapToResponse(entity);
    }

    public async Task<Guid> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var entity = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = GenerateSlug(request.Name),
            IconUrl = request.IconUrl,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _categoryData.AddCategoryAsync(entity);
    }

    public async Task<bool> UpdateCategoryAsync(Guid id, CreateCategoryRequest request)
    {
        var entity = new Category
        {
            Id = id,
            Name = request.Name,
            Slug = GenerateSlug(request.Name),
            IconUrl = request.IconUrl,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive
        };

        return await _categoryData.UpdateCategoryAsync(entity);
    }

    public Task<bool> DeleteCategoryAsync(Guid id) => _categoryData.DeleteCategoryAsync(id);

    public async Task<Guid> CreateSubcategoryAsync(CreateSubcategoryRequest request)
    {
        var entity = new Subcategory
        {
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
            Name = request.Name,
            Slug = GenerateSlug(request.Name),
            IconUrl = request.IconUrl,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _categoryData.AddSubcategoryAsync(entity);
    }

    public async Task<bool> UpdateSubcategoryAsync(Guid id, CreateSubcategoryRequest request)
    {
        var entity = new Subcategory
        {
            Id = id,
            CategoryId = request.CategoryId,
            Name = request.Name,
            Slug = GenerateSlug(request.Name),
            IconUrl = request.IconUrl,
            SortOrder = request.SortOrder,
            IsActive = request.IsActive
        };

        return await _categoryData.UpdateSubcategoryAsync(entity);
    }

    public Task<bool> DeleteSubcategoryAsync(Guid id) => _categoryData.DeleteSubcategoryAsync(id);

    private static CategoryResponse MapToResponse(Category c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Slug = c.Slug,
        IconUrl = c.IconUrl,
        SortOrder = c.SortOrder,
        IsActive = c.IsActive,
        Subcategories = c.Subcategories.Select(s => new SubcategoryResponse
        {
            Id = s.Id,
            CategoryId = s.CategoryId,
            Name = s.Name,
            Slug = s.Slug,
            IconUrl = s.IconUrl
        }).ToList()
    };

    private static string GenerateSlug(string name)
    {
        return name.ToLower()
                   .Trim()
                   .Replace(" ", "-")
                   .Replace("Ã¡", "a")
                   .Replace("Ã©", "e")
                   .Replace("Ã­", "i")
                   .Replace("Ã³", "o")
                   .Replace("Ãº", "u")
                   .Replace("Ã±", "n");
    }
}

