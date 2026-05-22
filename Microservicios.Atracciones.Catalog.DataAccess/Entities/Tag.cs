namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class Tag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<AttractionTag> AttractionTags { get; set; } = [];
}

