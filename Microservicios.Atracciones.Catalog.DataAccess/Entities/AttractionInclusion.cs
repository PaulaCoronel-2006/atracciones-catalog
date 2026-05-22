namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class AttractionInclusion
{
    public Guid AttractionId { get; set; }
    public Guid InclusionItemId { get; set; }
    public string Type { get; set; } = string.Empty;

    public virtual Attraction Attraction { get; set; } = null!;
    public virtual InclusionItem InclusionItem { get; set; } = null!;
}

