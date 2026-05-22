namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class TourItinerary
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AttractionId { get; set; }
    public short LanguageId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Overview { get; set; }
    public decimal? TotalDistanceKm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual Attraction Attraction { get; set; } = null!;
    public virtual Language Language { get; set; } = null!;
    public virtual ICollection<TourStop> Stops { get; set; } = [];
}

