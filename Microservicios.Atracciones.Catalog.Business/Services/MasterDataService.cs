using Microservicios.Atracciones.Catalog.Business.DTOs.Common;
using Microservicios.Atracciones.Catalog.Business.DTOs.Attraction;
using Microservicios.Atracciones.Catalog.Business.DTOs.Master;
using Microservicios.Atracciones.Catalog.Business.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;

namespace Microservicios.Atracciones.Catalog.Business.Services;

public class MasterDataService : IMasterDataService
{
    private readonly IMasterDataDataService _masterData;

    public MasterDataService(IMasterDataDataService masterData)
    {
        _masterData = masterData;
    }

    public async Task<IEnumerable<TagResponse>> GetTagsAsync()
    {
        var tags = await _masterData.GetAllTagsAsync();
        return tags.Select(MapToTagResponse);
    }

    public async Task<TagResponse?> GetTagByIdAsync(Guid id)
    {
        var tag = await _masterData.GetTagByIdAsync(id);
        return tag == null ? null : MapToTagResponse(tag);
    }

    public async Task<Guid> CreateTagAsync(CreateTagRequest request)
    {
        var tag = new DataAccess.Entities.Tag
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = GenerateSlug(request.Name)
        };
        return await _masterData.AddTagAsync(tag);
    }

    public async Task<bool> UpdateTagAsync(Guid id, CreateTagRequest request)
    {
        var tag = new DataAccess.Entities.Tag
        {
            Id = id,
            Name = request.Name,
            Slug = GenerateSlug(request.Name)
        };
        return await _masterData.UpdateTagAsync(tag);
    }

    public Task<bool> DeleteTagAsync(Guid id) => _masterData.DeleteTagAsync(id);

    public async Task<IEnumerable<InclusionResponse>> GetInclusionsAsync()
    {
        var inclusions = await _masterData.GetAllInclusionsAsync();
        return inclusions.Select(MapToInclusionResponse);
    }

    public async Task<InclusionResponse?> GetInclusionByIdAsync(Guid id)
    {
        var inclusion = await _masterData.GetInclusionByIdAsync(id);
        return inclusion == null ? null : MapToInclusionResponse(inclusion);
    }

    public async Task<Guid> CreateInclusionAsync(CreateInclusionRequest request)
    {
        var inclusion = new DataAccess.Entities.InclusionItem
        {
            Id = Guid.NewGuid(),
            DefaultText = request.DefaultText,
            IconSlug = request.IconSlug,
            LanguageId = request.LanguageId,
            CreatedAt = DateTime.UtcNow
        };
        return await _masterData.AddInclusionAsync(inclusion);
    }

    public async Task<bool> UpdateInclusionAsync(Guid id, CreateInclusionRequest request)
    {
        var inclusion = new DataAccess.Entities.InclusionItem
        {
            Id = id,
            DefaultText = request.DefaultText,
            IconSlug = request.IconSlug,
            LanguageId = request.LanguageId
        };
        return await _masterData.UpdateInclusionAsync(inclusion);
    }

    public Task<bool> DeleteInclusionAsync(Guid id) => _masterData.DeleteInclusionAsync(id);

    public async Task<IEnumerable<LanguageResponse>> GetLanguagesAsync()
    {
        var langs = await _masterData.GetAllLanguagesAsync();
        return langs.Select(l => new LanguageResponse
        {
            Id = l.Id,
            IsoCode = l.IsoCode,
            Name = l.Name
        });
    }

    public async Task<IEnumerable<TicketCategoryResponse>> GetTicketCategoriesAsync()
    {
        var cats = await _masterData.GetAllTicketCategoriesAsync();
        return cats.Select(MapToTicketCategoryResponse);
    }

    public async Task<TicketCategoryResponse?> GetTicketCategoryByIdAsync(Guid id)
    {
        var cat = await _masterData.GetTicketCategoryByIdAsync(id);
        return cat == null ? null : MapToTicketCategoryResponse(cat);
    }

    public async Task<Guid> CreateTicketCategoryAsync(CreateTicketCategoryRequest request)
    {
        var category = new DataAccess.Entities.TicketCategory
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            NameEn = request.NameEn,
            AgeRangeMin = request.AgeRangeMin,
            AgeRangeMax = request.AgeRangeMax,
            SortOrder = request.SortOrder,
            IsActive = true
        };
        return await _masterData.AddTicketCategoryAsync(category);
    }

    public async Task<bool> UpdateTicketCategoryAsync(Guid id, CreateTicketCategoryRequest request)
    {
        var category = new DataAccess.Entities.TicketCategory
        {
            Id = id,
            Name = request.Name,
            NameEn = request.NameEn,
            AgeRangeMin = request.AgeRangeMin,
            AgeRangeMax = request.AgeRangeMax,
            SortOrder = request.SortOrder,
            IsActive = true
        };
        return await _masterData.UpdateTicketCategoryAsync(category);
    }

    public Task<bool> DeleteTicketCategoryAsync(Guid id) => _masterData.DeleteTicketCategoryAsync(id);

    private static TicketCategoryResponse MapToTicketCategoryResponse(DataAccess.Entities.TicketCategory c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        NameEn = c.NameEn,
        AgeRangeMin = c.AgeRangeMin,
        AgeRangeMax = c.AgeRangeMax
    };

    private static TagResponse MapToTagResponse(DataAccess.Entities.Tag t) => new()
    {
        Id = t.Id,
        Name = t.Name,
        Slug = t.Slug
    };

    private static InclusionResponse MapToInclusionResponse(DataAccess.Entities.InclusionItem i) => new()
    {
        Id = i.Id,
        DefaultText = i.DefaultText,
        IconSlug = i.IconSlug
    };

    private static string GenerateSlug(string text)
    {
        return text.ToLower()
                   .Trim()
                   .Replace(" ", "-")
                   .Replace("Ã¡", "a").Replace("Ã©", "e").Replace("Ã­", "i").Replace("Ã³", "o").Replace("Ãº", "u")
                   .Replace("Ã±", "n");
    }
}

