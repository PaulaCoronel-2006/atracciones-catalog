namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

/// <summary>
/// CatÃ¡logo de Ã­tems de inclusiÃ³n/exclusiÃ³n reutilizables.
/// category: "included" | "not_included" | "optional" | "bring"
/// </summary>
public class InclusionItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? IconSlug { get; set; }
    public string DefaultText { get; set; } = string.Empty;
    public short LanguageId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Language Language { get; set; } = null!;
    public virtual ICollection<AttractionInclusion> AttractionInclusions { get; set; } = [];
}

