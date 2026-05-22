using Microservicios.Atracciones.Catalog.Business.Interfaces;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Models;

namespace Microservicios.Atracciones.Catalog.Business.Services;

public class LocationService : ILocationService
{
    private readonly ILocationDataService _locationData;

    public LocationService(ILocationDataService locationData)
    {
        _locationData = locationData;
    }

    public Task<IEnumerable<LocationNode>> GetHierarchyAsync() => _locationData.GetHierarchyAsync();

    public Task<LocationNode?> GetByIdAsync(Guid id) => _locationData.GetByIdAsync(id);

    public async Task<Guid> CreateAsync(CreateLocationRequest request)
    {
        var entity = new Location
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type,
            ParentId = request.ParentId,
            CountryCode = request.CountryCode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _locationData.AddLocationAsync(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, CreateLocationRequest request)
    {
        var entity = new Location
        {
            Id = id,
            Name = request.Name,
            Type = request.Type,
            ParentId = request.ParentId,
            CountryCode = request.CountryCode
        };

        return await _locationData.UpdateLocationAsync(entity);
    }

    public Task<bool> DeleteAsync(Guid id) => _locationData.DeleteLocationAsync(id);
}

