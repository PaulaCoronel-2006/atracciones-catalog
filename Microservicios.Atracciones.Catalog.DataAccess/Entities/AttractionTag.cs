namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

/// <summary>Tabla puente N-M entre Attraction y Tag.</summary>
public class AttractionTag
{
    public Guid AttractionId { get; set; }
    public Guid TagId { get; set; }

    public virtual Attraction Attraction { get; set; } = null!;
    public virtual Tag Tag { get; set; } = null!;
}

