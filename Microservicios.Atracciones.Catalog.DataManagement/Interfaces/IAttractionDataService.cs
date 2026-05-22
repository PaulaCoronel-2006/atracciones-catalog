using Microservicios.Atracciones.Catalog.DataAccess.Common;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataManagement.Models;

namespace Microservicios.Atracciones.Catalog.DataManagement.Interfaces;

public interface IAttractionDataService
{
    Task<PagedResult<AttractionNode>> SearchAttractionsAsync(QueryFilters filters);
    Task<AttractionNode?> GetAttractionBySlugAsync(string slug, short? languageId = null);
    Task<Attraction?> GetByIdAsync(Guid id);
    Task<Attraction?> GetFullByIdAsync(Guid id);
    Task<IEnumerable<AttractionNode>> GetTopRatedAsync(int count);
    Task<Guid> AddAttractionAsync(Attraction attraction);
    Task<bool> UpdateAsync(Attraction attraction);
    Task<bool> DeleteAsync(Guid id);
}

