namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

/// <summary>
/// CatÃ¡logo de idiomas ISO 639-1.
/// </summary>
public class Language
{
    public short Id { get; set; }
    public string IsoCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // NavegaciÃ³n
    public virtual ICollection<AttractionTranslation> AttractionTranslations { get; set; } = [];
    public virtual ICollection<AttractionLanguage> AttractionLanguages { get; set; } = [];
    public virtual ICollection<AudioGuide> AudioGuides { get; set; } = [];
    public virtual ICollection<TourItinerary> TourItineraries { get; set; } = [];
    public virtual ICollection<ProductTranslation> ProductTranslations { get; set; } = [];
}

