using Microsoft.EntityFrameworkCore;
using Microservicios.Atracciones.Catalog.DataAccess.Common;
using Microservicios.Atracciones.Catalog.DataAccess.Context;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;

namespace Microservicios.Atracciones.Catalog.DataAccess.Queries;

public class AttractionQueries : IAttractionQueries
{
    private readonly AtraccionDbContext _context;

    public AttractionQueries(AtraccionDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Attraction>> SearchAttractionsAsync(QueryFilters filters)
    {
        var query = _context.Attractions
            .AsNoTracking()
            .Include(a => a.Media.Where(m => m.IsMain))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.SearchTerm))
        {
            query = query.Where(a => a.Name.Contains(filters.SearchTerm) ||
                                    (a.DescriptionShort != null && a.DescriptionShort.Contains(filters.SearchTerm)));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((filters.PageNumber - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .ToListAsync();

        return new PagedResult<Attraction>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filters.PageNumber,
            PageSize = filters.PageSize
        };
    }

    public async Task<IEnumerable<Attraction>> GetTopRatedAttractionsAsync(int count)
    {
        return await _context.Attractions
            .AsNoTracking()
            .OrderByDescending(a => a.RatingAverage)
            .Take(count)
            .ToListAsync();
    }
}
