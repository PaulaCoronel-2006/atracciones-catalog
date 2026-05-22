using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Models;

namespace Microservicios.Atracciones.Catalog.DataManagement.Services;

public class LocationDataService : ILocationDataService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public LocationDataService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<LocationNode?> GetByIdAsync(Guid id)
    {
        var entity = await _uow.Locations.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<LocationNode>(entity);
    }

    public async Task<IEnumerable<LocationNode>> GetHierarchyAsync()
    {
        // Traemos todas las locaciones (asumiendo que no son miles masivas)
        var allLocations = await _uow.Locations.Query()
                              .OrderBy(l => l.Name)
                              .ToListAsync();

        var nodes = _mapper.Map<List<LocationNode>>(allLocations);

        // Construir el Ã¡rbol
        var lookup = nodes.ToDictionary(n => n.Id);
        var roots = new List<LocationNode>();

        foreach (var node in nodes)
        {
            if (node.ParentId.HasValue && lookup.TryGetValue(node.ParentId.Value, out var parent))
            {
                parent.Children.Add(node);
            }
            else
            {
                roots.Add(node);
            }
        }

        return roots;
    }

    public async Task<Guid> AddLocationAsync(Location location)
    {
        await _uow.Locations.AddAsync(location);
        await _uow.CompleteAsync();
        return location.Id;
    }

    public async Task<bool> UpdateLocationAsync(Location location)
    {
        var entity = await _uow.Locations.GetByIdAsync(location.Id);
        if (entity == null) return false;

        entity.Name = location.Name;
        entity.Type = location.Type;
        entity.ParentId = location.ParentId;
        entity.CountryCode = location.CountryCode;
        entity.UpdatedAt = DateTime.UtcNow;

        _uow.Locations.Update(entity);
        return await _uow.CompleteAsync() > 0;
    }

    public async Task<bool> DeleteLocationAsync(Guid id)
    {
        var entity = await _uow.Locations.GetByIdAsync(id);
        if (entity == null) return false;

        _uow.Locations.Delete(entity);
        return await _uow.CompleteAsync() > 0;
    }
}

