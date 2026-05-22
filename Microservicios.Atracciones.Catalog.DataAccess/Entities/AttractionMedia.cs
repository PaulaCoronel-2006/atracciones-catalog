namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class AttractionMedia
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AttractionId { get; set; }
    public short MediaTypeId { get; set; }      // 1=image, 2=video, 3=document
    public string Url { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Title { get; set; }
    public short? LanguageId { get; set; }       // NULL = todos los idiomas
    public bool IsMain { get; set; } = false;
    public short SortOrder { get; set; } = 0;
    public int? FileSizeKb { get; set; }
    public int? DurationSecs { get; set; }       // Para videos
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Attraction Attraction { get; set; } = null!;
    public virtual MediaType MediaType { get; set; } = null!;
    public virtual Language? Language { get; set; }
}
