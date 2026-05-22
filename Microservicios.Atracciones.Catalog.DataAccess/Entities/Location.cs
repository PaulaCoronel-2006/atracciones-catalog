using Microservicios.Atracciones.Catalog.DataAccess.Common;

namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

/// <summary>
/// Modelo jerÃ¡rquico representativo de entidades geogrÃ¡ficas (paÃ­ses, estados, ciudades).
/// </summary>
public class Location : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;  // "country", "state", "city", etc.
    public Guid? ParentId { get; set; }
    public string? CountryCode { get; set; }

    // NavegaciÃ³n
    public virtual Location? Parent { get; set; }
    public virtual ICollection<Location> Children { get; set; } = [];
    public virtual ICollection<Attraction> Attractions { get; set; } = [];
}

