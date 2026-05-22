using Microservicios.Atracciones.Catalog.DataAccess.Entities;

namespace Microservicios.Atracciones.Catalog.DataManagement.Models;

/// <summary>
/// Representa una vista densa y consolidada de una AtracciÃ³n 
/// (agrupando traducciones, media y descripciones principales de su ubicaciÃ³n).
/// Ãštil para la lÃ³gica de dominio superior (Servicios de Business).
/// </summary>
public class AttractionNode
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? DescriptionShort { get; set; }
    public string? DescriptionFull { get; set; }
    public decimal RatingAverage { get; set; }
    public int RatingCount { get; set; }
    public string? DifficultyLevel { get; set; }
    public Guid? ManagedById { get; set; }
    public decimal? StartingPrice { get; set; }
    public string? Address { get; set; }
    public string? MeetingPoint { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public int ModalityCount { get; set; }
    
    // JerarquÃ­a directa aplanada
    public Guid LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string LocationCountryCode { get; set; } = string.Empty;

    public Guid SubcategoryId { get; set; }
    public string SubcategoryName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;

    // Elementos multimedia principales
    public List<MediaNode> MediaGallery { get; set; } = [];
    public List<TagNode> Tags { get; set; } = [];
    public List<InclusionNode> Inclusions { get; set; } = [];
    public List<GuideLanguageNode> GuideLanguages { get; set; } = [];
}

public class TagNode
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class InclusionNode
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = "included";
}

public class GuideLanguageNode
{
    public short LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string GuideType { get; set; } = "live";
}

public class MediaNode
{
    public string Url { get; set; } = string.Empty;
    public string? Title { get; set; }
    public bool IsMain { get; set; }
    public short SortOrder { get; set; }
}

