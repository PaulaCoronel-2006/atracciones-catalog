namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class AttractionTranslation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AttractionId { get; set; }
    public short LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? DescriptionShort { get; set; }
    public string? DescriptionFull { get; set; }
    public string? MeetingPoint { get; set; }

    public virtual Attraction Attraction { get; set; } = null!;
    public virtual Language Language { get; set; } = null!;
}

