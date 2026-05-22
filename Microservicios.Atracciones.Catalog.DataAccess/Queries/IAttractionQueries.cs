using Microservicios.Atracciones.Catalog.DataAccess.Common;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;

namespace Microservicios.Atracciones.Catalog.DataAccess.Queries;

public interface IAttractionQueries
{
    Task<PagedResult<Attraction>> SearchAttractionsAsync(QueryFilters filters);
    Task<IEnumerable<Attraction>> GetTopRatedAttractionsAsync(int count);
}
