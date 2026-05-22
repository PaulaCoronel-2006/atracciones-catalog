namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class TourStop
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ItineraryId { get; set; }
    public short StopNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public short? DurationMinutes { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    /// <summary>"included" | "optional" | "excluded"</summary>
    public string? AdmissionType { get; set; }

    public virtual TourItinerary Itinerary { get; set; } = null!;
    public virtual ICollection<TourStopMedia> Media { get; set; } = [];
}

