using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microservicios.Atracciones.Catalog.DataAccess.Common;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Models;

namespace Microservicios.Atracciones.Catalog.DataManagement.Services;

public class AttractionDataService : IAttractionDataService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AttractionDataService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AttractionNode?> GetAttractionBySlugAsync(string slug, short? languageId = null)
    {
        var attraction = await _unitOfWork.Attractions.Query()
            .Include(a => a.Location)
            .Include(a => a.Subcategory)
            .ThenInclude(s => s.Category)
            .Include(a => a.Media)
            .Include(a => a.Tags).ThenInclude(t => t.Tag)
            .Include(a => a.Inclusions).ThenInclude(i => i.InclusionItem)
            .Include(a => a.Languages)
            .FirstOrDefaultAsync(a => a.Slug == slug && a.DeletedAt == null);

        if (attraction == null) return null;

        if (languageId.HasValue)
        {
            var translation = attraction.Translations.FirstOrDefault(t => t.LanguageId == languageId.Value);
            if (translation != null)
            {
                attraction.Name = translation.Name;
                attraction.DescriptionShort = translation.DescriptionShort ?? attraction.DescriptionShort;
                attraction.DescriptionFull = translation.DescriptionFull ?? attraction.DescriptionFull;
            }
        }

        var node = _mapper.Map<AttractionNode>(attraction);

        node.Tags = attraction.Tags.Select(at => new TagNode
        {
            Id = at.Tag?.Id ?? at.TagId,
            Name = at.Tag?.Name ?? ""
        }).ToList();

        node.Inclusions = attraction.Inclusions.Select(ai => new InclusionNode
        {
            Id = ai.InclusionItem?.Id ?? Guid.Empty,
            Name = ai.InclusionItem?.DefaultText ?? "",
            Description = ai.InclusionItem?.IconSlug,
            Type = ai.Type
        }).ToList();

        node.GuideLanguages = attraction.Languages.Select(l => new GuideLanguageNode
        {
            LanguageId = l.LanguageId,
            GuideType = l.GuideType,
            Name = l.LanguageId switch { 1 => "EspaÃ±ol", 2 => "InglÃ©s", 3 => "FrancÃ©s", 4 => "PortuguÃ©s", _ => $"Idioma {l.LanguageId}" }
        }).ToList();

        return node;
    }

    public async Task<AttractionNode?> GetAttractionByIdNodeAsync(Guid id, short? languageId = null)
    {
        var attraction = await _unitOfWork.Attractions.Query()
            .Include(a => a.Location)
            .Include(a => a.Subcategory)
            .ThenInclude(s => s.Category)
            .Include(a => a.Media)
            .Include(a => a.Tags).ThenInclude(t => t.Tag)
            .Include(a => a.Inclusions).ThenInclude(i => i.InclusionItem)
            .Include(a => a.Languages)
            .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null);

        if (attraction == null) return null;

        if (languageId.HasValue)
        {
            var translation = attraction.Translations.FirstOrDefault(t => t.LanguageId == languageId.Value);
            if (translation != null)
            {
                attraction.Name = translation.Name;
                attraction.DescriptionShort = translation.DescriptionShort ?? attraction.DescriptionShort;
                attraction.DescriptionFull = translation.DescriptionFull ?? attraction.DescriptionFull;
            }
        }

        var node = _mapper.Map<AttractionNode>(attraction);

        node.Tags = attraction.Tags.Select(at => new TagNode
        {
            Id = at.Tag?.Id ?? at.TagId,
            Name = at.Tag?.Name ?? ""
        }).ToList();

        node.Inclusions = attraction.Inclusions.Select(ai => new InclusionNode
        {
            Id = ai.InclusionItem?.Id ?? Guid.Empty,
            Name = ai.InclusionItem?.DefaultText ?? "",
            Description = ai.InclusionItem?.IconSlug,
            Type = ai.Type
        }).ToList();

        node.GuideLanguages = attraction.Languages.Select(l => new GuideLanguageNode
        {
            LanguageId = l.LanguageId,
            GuideType = l.GuideType,
            Name = l.LanguageId switch { 1 => "Español", 2 => "Inglés", 3 => "Francés", 4 => "Portugués", _ => $"Idioma {l.LanguageId}" }
        }).ToList();

        return node;
    }

    public async Task<IEnumerable<AttractionNode>> GetTopRatedAsync(int count)
    {
        var attractions = await _unitOfWork.Attractions.Query()
            .Include(a => a.Location)
            .Include(a => a.Subcategory)
            .ThenInclude(s => s.Category)
            .Include(a => a.Media.Where(m => m.IsMain))
            .Include(a => a.ProductOptions)
                .ThenInclude(p => p.PriceTiers)
            .Where(a => a.IsPublished && a.IsActive && a.DeletedAt == null)
            .OrderByDescending(a => a.RatingAverage)
            .Take(count)
            .ToListAsync();

        var nodes = _mapper.Map<IEnumerable<AttractionNode>>(attractions).ToList();

        foreach (var node in nodes)
        {
            var entity = attractions.First(i => i.Id == node.Id);
            var prices = entity.ProductOptions
                               .SelectMany(po => po.PriceTiers)
                               .Select(pt => pt.Price)
                               .ToList();

            if (prices.Any())
            {
                node.StartingPrice = prices.Min();
            }
            node.ModalityCount = entity.ProductOptions.Count;
        }

        return nodes;
    }

    public async Task<PagedResult<AttractionNode>> SearchAttractionsAsync(QueryFilters filters)
    {
        IQueryable<Attraction> query = _unitOfWork.Attractions.Query()
            .Include(a => a.Location)
            .Include(a => a.Subcategory).ThenInclude(s => s.Category)
            .Include(a => a.Media.Where(m => m.IsMain))
            .Include(a => a.ProductOptions)
                .ThenInclude(p => p.PriceTiers)
            .Where(a => a.DeletedAt == null);
            
        if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
        {
            query = query.Where(a => a.Name.Contains(filters.SearchTerm) || (a.DescriptionShort != null && a.DescriptionShort.Contains(filters.SearchTerm)));
        }

        if (filters is AttractionQueryFilters attrFilters)
        {
            if (attrFilters.IsPublished.HasValue)
                query = query.Where(a => a.IsPublished == attrFilters.IsPublished.Value);

            if (attrFilters.IsActive.HasValue)
                query = query.Where(a => a.IsActive == attrFilters.IsActive.Value);

            if (attrFilters.ManagedById.HasValue)
                query = query.Where(a => a.ManagedById == attrFilters.ManagedById.Value);

            if (attrFilters.LocationId.HasValue)
            {
                query = query.Where(a => a.LocationId == attrFilters.LocationId.Value || 
                                       (a.Location != null && a.Location.ParentId == attrFilters.LocationId.Value) || 
                                       (a.Location != null && a.Location.Parent != null && a.Location.Parent.ParentId == attrFilters.LocationId.Value));
            }

            if (!string.IsNullOrEmpty(attrFilters.CategorySlug))
                query = query.Where(a => a.Subcategory.Category.Slug == attrFilters.CategorySlug);

            if (attrFilters.CategoryId.HasValue)
                query = query.Where(a => a.Subcategory.CategoryId == attrFilters.CategoryId.Value);

            if (!string.IsNullOrEmpty(attrFilters.TagIds))
            {
                var tagIdList = attrFilters.TagIds.Split(',').Select(id => Guid.Parse(id.Trim())).ToList();
                query = query.Where(a => a.Tags.Any(at => tagIdList.Contains(at.TagId)));
            }
            else if (attrFilters.TagId.HasValue)
            {
                query = query.Where(a => a.Tags.Any(at => at.TagId == attrFilters.TagId.Value));
            }

            if (attrFilters.SubcategoryId.HasValue)
                query = query.Where(a => a.SubcategoryId == attrFilters.SubcategoryId.Value);

            if (!string.IsNullOrEmpty(attrFilters.LanguageIds))
            {
                var langIdList = attrFilters.LanguageIds.Split(',').Select(id => short.Parse(id.Trim())).ToList();
                query = query.Where(a => a.Languages.Any(al => langIdList.Contains(al.LanguageId)));
            }
            else if (attrFilters.LanguageId.HasValue)
            {
                query = query.Where(a => a.Languages.Any(al => al.LanguageId == attrFilters.LanguageId.Value));
            }

            if (!string.IsNullOrEmpty(attrFilters.DifficultyLevels))
            {
                var diffList = attrFilters.DifficultyLevels.Split(',').Select(d => d.Trim().ToLower()).ToList();
                query = query.Where(a => !string.IsNullOrEmpty(a.DifficultyLevel) && diffList.Contains(a.DifficultyLevel.ToLower()));
            }
            else if (!string.IsNullOrEmpty(attrFilters.DifficultyLevel))
            {
                query = query.Where(a => a.DifficultyLevel == attrFilters.DifficultyLevel);
            }

            if (attrFilters.MinRating.HasValue)
                query = query.Where(a => a.RatingAverage >= attrFilters.MinRating.Value);
            
            if (attrFilters.MinPrice.HasValue)
                query = query.Where(a => a.ProductOptions.Any(po => po.PriceTiers.Any(pt => pt.Price >= attrFilters.MinPrice.Value)));

            if (attrFilters.MaxPrice.HasValue)
                query = query.Where(a => a.ProductOptions.Any(po => po.PriceTiers.Any(pt => pt.Price <= attrFilters.MaxPrice.Value)));
        }

        var totalCount = await query.CountAsync();
        
        var items = await query.OrderByDescending(x => x.CreatedAt)
                               .Skip(filters.Offset)
                               .Take(filters.PageSize)
                               .ToListAsync();

        var nodes = _mapper.Map<IEnumerable<AttractionNode>>(items).ToList();

        foreach (var node in nodes)
        {
            var entity = items.First(i => i.Id == node.Id);
            var prices = entity.ProductOptions
                               .SelectMany(po => po.PriceTiers)
                               .Select(pt => pt.Price)
                               .ToList();

            if (prices.Any())
            {
                node.StartingPrice = prices.Min();
            }
            node.ModalityCount = entity.ProductOptions.Count;
        }

        return new PagedResult<AttractionNode>
        {
            Items = nodes,
            TotalCount = totalCount,
            PageNumber = filters.PageNumber,
            PageSize = filters.PageSize
        };
    }

    public async Task<Guid> AddAttractionAsync(Attraction attraction)
    {
        await _unitOfWork.Attractions.AddAsync(attraction);
        await _unitOfWork.CompleteAsync();
        return attraction.Id;
    }

    public async Task<Attraction?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.Attractions.GetByIdAsync(id);
    }

    public async Task<Attraction?> GetFullByIdAsync(Guid id)
    {
        return await _unitOfWork.Attractions.Query()
            .Include(a => a.Location).ThenInclude(l => l!.Parent).ThenInclude(p => p!.Parent)
            .Include(a => a.Subcategory).ThenInclude(s => s!.Category)
            .Include(a => a.Media)
            .Include(a => a.Tags).ThenInclude(t => t.Tag)
            .Include(a => a.Inclusions).ThenInclude(i => i!.InclusionItem)
            .Include(a => a.Languages)
            .Include(a => a.Itineraries).ThenInclude(i => i.Stops)
            .Include(a => a.ProductOptions)
                .ThenInclude(p => p.PriceTiers)
                    .ThenInclude(pt => pt.TicketCategory)
            .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null);
    }

    public async Task<bool> UpdateAsync(Attraction attraction)
    {
        var existing = await _unitOfWork.Attractions.GetByIdAsync(attraction.Id);
        if (existing == null) return false;

        existing.Name = attraction.Name;
        existing.LocationId = attraction.LocationId;
        existing.SubcategoryId = attraction.SubcategoryId;
        existing.DescriptionShort = attraction.DescriptionShort;
        existing.DescriptionFull = attraction.DescriptionFull;
        existing.Address = attraction.Address;
        existing.Latitude = attraction.Latitude;
        existing.Longitude = attraction.Longitude;
        existing.MeetingPoint = attraction.MeetingPoint;
        existing.DifficultyLevel = attraction.DifficultyLevel;
        existing.IsPublished = attraction.IsPublished;
        existing.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Attractions.Update(existing);
        return await _unitOfWork.CompleteAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _unitOfWork.Attractions.GetByIdAsync(id);
        if (existing == null) return false;

        existing.IsActive = false;
        existing.DeletedAt = DateTime.UtcNow;

        _unitOfWork.Attractions.Update(existing);
        return await _unitOfWork.CompleteAsync() > 0;
    }
}

