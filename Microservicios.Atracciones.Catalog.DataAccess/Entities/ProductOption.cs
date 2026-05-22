using Microservicios.Atracciones.Catalog.DataAccess.Common;

namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class ProductOption : BaseEntity
{
    public Guid AttractionId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public string? DurationDescription { get; set; }    // "Aprox. 3 horas"
    public int CancelPolicyHours { get; set; } = 24;
    public string? CancelPolicyText { get; set; }
    public short? MaxGroupSize { get; set; }
    public short MinParticipants { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public bool IsPrivate { get; set; } = false;
    public short SortOrder { get; set; } = 0;

    // NavegaciÃ³n
    public virtual Attraction Attraction { get; set; } = null!;
    public virtual ICollection<ProductTranslation> Translations { get; set; } = [];
    public virtual ICollection<PriceTier> PriceTiers { get; set; } = [];
}
