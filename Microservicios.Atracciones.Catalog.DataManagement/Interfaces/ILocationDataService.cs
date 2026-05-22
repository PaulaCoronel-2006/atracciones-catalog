using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataManagement.Models;

namespace Microservicios.Atracciones.Catalog.DataManagement.Interfaces;

public interface ILocationDataService
{
    Task<IEnumerable<LocationNode>> GetHierarchyAsync();
    Task<LocationNode?> GetByIdAsync(Guid id);
    Task<Guid> AddLocationAsync(Location location);
    Task<bool> UpdateLocationAsync(Location location);
    Task<bool> DeleteLocationAsync(Guid id);
}

