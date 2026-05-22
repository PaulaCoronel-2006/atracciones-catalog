using Microservicios.Atracciones.Catalog.DataAccess.Common;

namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class Attraction : BaseEntity
{
    public Guid LocationId { get; set; }
    public Guid SubcategoryId { get; set; }
    public string Slug { get; set; } = string.Empty;

    // Contenido base (idioma por defecto)
    public string Name { get; set; } = string.Empty;
    public string? DescriptionShort { get; set; }
    public string? DescriptionFull { get; set; }

    // LocalizaciÃ³n
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? MeetingPoint { get; set; }

    // MÃ©tricas
    public decimal RatingAverage { get; set; } = 0.00m;
    public int RatingCount { get; set; } = 0;

    // InformaciÃ³n operativa
    public short? MinAge { get; set; }
    public short? MaxGroupSize { get; set; }
    public string? DifficultyLevel { get; set; }       // "easy" | "moderate" | "hard"

    // Control
    public bool IsActive { get; set; } = true;
    public bool IsPublished { get; set; } = false;
    public Guid? ManagedById { get; set; }
    public DateTime? DeletedAt { get; set; }

    // NavegaciÃ³n
    public virtual Location Location { get; set; } = null!;
    public virtual Subcategory Subcategory { get; set; } = null!;
    public virtual ICollection<AttractionTranslation> Translations { get; set; } = [];
    public virtual ICollection<AttractionLanguage> Languages { get; set; } = [];
    public virtual ICollection<AttractionMedia> Media { get; set; } = [];
    public virtual ICollection<AttractionTag> Tags { get; set; } = [];
    public virtual ICollection<AttractionInclusion> Inclusions { get; set; } = [];
    public virtual ICollection<AudioGuide> AudioGuides { get; set; } = [];
    public virtual ICollection<TourItinerary> Itineraries { get; set; } = [];
    public virtual ICollection<ProductOption> ProductOptions { get; set; } = [];
}
