using Microservicios.Atracciones.Catalog.DataManagement.Models;

namespace Microservicios.Atracciones.Catalog.Business.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<LocationNode>> GetHierarchyAsync();
    Task<LocationNode?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CreateLocationRequest request);
    Task<bool> UpdateAsync(Guid id, CreateLocationRequest request);
    Task<bool> DeleteAsync(Guid id);
}

public class CreateLocationRequest
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "city";
    public Guid? ParentId { get; set; }
    public string? CountryCode { get; set; }
}

