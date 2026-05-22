namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class AudioGuide
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AttractionId { get; set; }
    public short LanguageId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? TotalDurationSecs { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual Attraction Attraction { get; set; } = null!;
    public virtual Language Language { get; set; } = null!;
    public virtual ICollection<AudioGuideStop> Stops { get; set; } = [];
}

