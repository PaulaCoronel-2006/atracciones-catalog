namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

/// <summary>
/// Indica en quÃ© idiomas estÃ¡ disponible la guÃ­a de la atracciÃ³n
/// y de quÃ© tipo es (en vivo, audio, escrita, app).
/// </summary>
public class AttractionLanguage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AttractionId { get; set; }
    public short LanguageId { get; set; }

    /// <summary>"live" | "audio" | "written" | "app"</summary>
    public string GuideType { get; set; } = string.Empty;

    public virtual Attraction Attraction { get; set; } = null!;
    public virtual Language Language { get; set; } = null!;
}

