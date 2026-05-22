using Microsoft.EntityFrameworkCore;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;

namespace Microservicios.Atracciones.Catalog.DataManagement.Services;

public class CategoryDataService : ICategoryDataService
{
    private readonly IUnitOfWork _uow;

    public CategoryDataService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Category>> GetAllWithSubcategoriesAsync()
    {
        return await _uow.Categories.Query()
            .Include(c => c.Subcategories)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _uow.Categories.Query()
            .Include(c => c.Subcategories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Guid> AddCategoryAsync(Category category)
    {
        await _uow.Categories.AddAsync(category);
        await _uow.CompleteAsync();
        return category.Id;
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        var entity = await _uow.Categories.GetByIdAsync(category.Id);
        if (entity == null) return false;

        entity.Name = category.Name;
        entity.Slug = category.Slug;
        entity.IconUrl = category.IconUrl;
        entity.SortOrder = category.SortOrder;
        entity.IsActive = category.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

        _uow.Categories.Update(entity);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var entity = await _uow.Categories.GetByIdAsync(id);
        if (entity == null) return false;

        _uow.Categories.Delete(entity);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<Guid> AddSubcategoryAsync(Subcategory subcategory)
    {
        await _uow.Subcategories.AddAsync(subcategory);
        await _uow.CompleteAsync();
        return subcategory.Id;
    }

    public async Task<bool> UpdateSubcategoryAsync(Subcategory subcategory)
    {
        var entity = await _uow.Subcategories.GetByIdAsync(subcategory.Id);
        if (entity == null) return false;

        entity.Name = subcategory.Name;
        entity.Slug = subcategory.Slug;
        entity.CategoryId = subcategory.CategoryId;
        entity.IconUrl = subcategory.IconUrl;
        entity.SortOrder = subcategory.SortOrder;
        entity.IsActive = subcategory.IsActive;
        entity.UpdatedAt = DateTime.UtcNow;

        _uow.Subcategories.Update(entity);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<bool> DeleteSubcategoryAsync(Guid id)
    {
        var entity = await _uow.Subcategories.GetByIdAsync(id);
        if (entity == null) return false;

        _uow.Subcategories.Delete(entity);
        return await _uow.CompleteAsync() > 0;
    }
}

