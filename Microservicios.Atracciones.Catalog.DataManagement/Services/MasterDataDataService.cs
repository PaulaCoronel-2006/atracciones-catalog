using Microsoft.EntityFrameworkCore;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;

namespace Microservicios.Atracciones.Catalog.DataManagement.Services;

public class MasterDataDataService : IMasterDataDataService
{
    private readonly IUnitOfWork _uow;

    public MasterDataDataService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<Tag>> GetAllTagsAsync()
    {
        return await _uow.Tags.Query().OrderBy(t => t.Name).ToListAsync();
    }

    public async Task<Tag?> GetTagByIdAsync(Guid id) => await _uow.Tags.GetByIdAsync(id);

    public async Task<Guid> AddTagAsync(Tag tag)
    {
        await _uow.Tags.AddAsync(tag);
        await _uow.CompleteAsync();
        return tag.Id;
    }

    public async Task<bool> UpdateTagAsync(Tag tag)
    {
        _uow.Tags.Update(tag);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<bool> DeleteTagAsync(Guid id)
    {
        var entity = await _uow.Tags.GetByIdAsync(id);
        if (entity == null) return false;
        _uow.Tags.Delete(entity);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<IEnumerable<InclusionItem>> GetAllInclusionsAsync()
    {
        return await _uow.InclusionItems.Query().OrderBy(i => i.DefaultText).ToListAsync();
    }

    public async Task<InclusionItem?> GetInclusionByIdAsync(Guid id) => await _uow.InclusionItems.GetByIdAsync(id);

    public async Task<Guid> AddInclusionAsync(InclusionItem inclusion)
    {
        await _uow.InclusionItems.AddAsync(inclusion);
        await _uow.CompleteAsync();
        return inclusion.Id;
    }

    public async Task<bool> UpdateInclusionAsync(InclusionItem inclusion)
    {
        _uow.InclusionItems.Update(inclusion);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<bool> DeleteInclusionAsync(Guid id)
    {
        var entity = await _uow.InclusionItems.GetByIdAsync(id);
        if (entity == null) return false;
        _uow.InclusionItems.Delete(entity);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
    {
        return await _uow.Languages.Query().Where(l => l.IsActive).OrderBy(l => l.Name).ToListAsync();
    }

    public async Task<IEnumerable<TicketCategory>> GetAllTicketCategoriesAsync()
    {
        return await _uow.TicketCategories.Query().Where(t => t.IsActive).OrderBy(t => t.SortOrder).ToListAsync();
    }

    public async Task<TicketCategory?> GetTicketCategoryByIdAsync(Guid id) => await _uow.TicketCategories.GetByIdAsync(id);

    public async Task<Guid> AddTicketCategoryAsync(TicketCategory category)
    {
        await _uow.TicketCategories.AddAsync(category);
        await _uow.CompleteAsync();
        return category.Id;
    }

    public async Task<bool> UpdateTicketCategoryAsync(TicketCategory category)
    {
        _uow.TicketCategories.Update(category);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<bool> DeleteTicketCategoryAsync(Guid id)
    {
        var entity = await _uow.TicketCategories.GetByIdAsync(id);
        if (entity == null) return false;
        
        // Logical delete or physical delete based on how IsActive is used, but typically Physical or set IsActive=false
        entity.IsActive = false;
        _uow.TicketCategories.Update(entity);
        return await _uow.CompleteAsync() > 0;
    }
}

