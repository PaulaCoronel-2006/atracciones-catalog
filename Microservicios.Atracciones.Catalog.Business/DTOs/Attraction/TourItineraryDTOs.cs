namespace Microservicios.Atracciones.Catalog.Business.DTOs.Attraction;

public class ItineraryResponse
{
    public Guid Id { get; set; }
    public short LanguageId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? TotalDistanceKm { get; set; }
    public int? TotalDurationMinutes { get; set; }
    public List<TourStopResponse> Stops { get; set; } = [];
}

public class TourStopResponse
{
    public Guid Id { get; set; }
    public short StopNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public string? AdmissionType { get; set; } // 'included', 'optional', 'excluded'
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}

public class CreateItineraryRequest
{
    public short LanguageId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? TotalDistanceKm { get; set; }
    public int? TotalDurationMinutes { get; set; }
}

public class CreateTourStopRequest
{
    public short StopNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public string? AdmissionType { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}

