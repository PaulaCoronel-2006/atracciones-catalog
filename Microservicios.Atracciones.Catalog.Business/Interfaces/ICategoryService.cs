using Microservicios.Atracciones.Catalog.Business.DTOs.Category;

namespace Microservicios.Atracciones.Catalog.Business.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetAllAsync();
    Task<CategoryResponse?> GetByIdAsync(Guid id);
    Task<Guid> CreateCategoryAsync(CreateCategoryRequest request);
    Task<bool> UpdateCategoryAsync(Guid id, CreateCategoryRequest request);
    Task<bool> DeleteCategoryAsync(Guid id);

    Task<Guid> CreateSubcategoryAsync(CreateSubcategoryRequest request);
    Task<bool> UpdateSubcategoryAsync(Guid id, CreateSubcategoryRequest request);
    Task<bool> DeleteSubcategoryAsync(Guid id);
}

