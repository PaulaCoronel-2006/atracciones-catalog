namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class AudioGuideStop
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AudioGuideId { get; set; }
    public short StopNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AudioUrl { get; set; } = string.Empty;
    public int? DurationSecs { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? ImageUrl { get; set; }

    public virtual AudioGuide AudioGuide { get; set; } = null!;
}

