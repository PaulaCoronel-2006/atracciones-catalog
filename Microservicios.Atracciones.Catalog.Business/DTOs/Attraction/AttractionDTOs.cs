using Microservicios.Atracciones.Catalog.Business.DTOs.Inventory;

namespace Microservicios.Atracciones.Catalog.Business.DTOs.Attraction;

/// <summary>Detalle extendido para el formulario de ediciÃ³n administrativa.</summary>
public class AttractionFullEditionResponse : AttractionDetailResponse
{
    public Guid CountryId { get; set; }
    public Guid StateId { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
}

/// <summary>PeticiÃ³n para activar/desactivar lÃ³gicamente una atracciÃ³n.</summary>
public class ToggleAttractionActiveRequest
{
    [System.ComponentModel.DataAnnotations.Required]
    public bool IsActive { get; set; }
}

/// <summary>Resultado compacto para listas y buscadores.</summary>
public class AttractionSummaryResponse
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? DescriptionShort { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string LocationCountryCode { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public string? SubcategoryName { get; set; }
    public decimal RatingAverage { get; set; }
    public int RatingCount { get; set; }
    public string? DifficultyLevel { get; set; }
    public string? MainImageUrl { get; set; }
    public decimal? StartingPrice { get; set; }       // Precio base (menor PriceTier activo)
    public string CurrencyCode { get; set; } = "USD";
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public int ModalityCount { get; set; }
}

/// <summary>Detalle completo de una atracciÃ³n (incluyendo productos y media).</summary>
public class AttractionDetailResponse
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? DescriptionShort { get; set; }
    public string? DescriptionFull { get; set; }
    public string? DifficultyLevel { get; set; }
    public decimal RatingAverage { get; set; }
    public int RatingCount { get; set; }
    public short? MinAge { get; set; }
    public short? MaxGroupSize { get; set; }
    public string? Address { get; set; }
    public string? MeetingPoint { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public Guid LocationId { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string LocationCountryCode { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;
    public string SubcategoryName { get; set; } = string.Empty;

    public List<MediaResponse> Gallery { get; set; } = [];
    public List<ProductResponse> Products { get; set; } = [];
    public List<TagResponse> Tags { get; set; } = [];
    public List<InclusionResponse> Inclusions { get; set; } = [];
    public List<GuideLanguageResponse> GuideLanguages { get; set; } = [];
    public ItineraryResponse? Itinerary { get; set; }
}

public partial class TagResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}

public class InclusionResponse
{
    public Guid Id { get; set; }
    public Guid InclusionItemId { get; set; } // ID original para ediciÃ³n
    public string Name { get; set; } = string.Empty;
    public string? DefaultText { get; set; }
    public string? IconSlug { get; set; }
    public string? Description { get; set; }
    public string Type { get; set; } = "included";
}

public class GuideLanguageResponse
{
    public short LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string GuideType { get; set; } = "live";
}

public class MediaResponse
{
    public string Url { get; set; } = string.Empty;
    public string? Title { get; set; }
    public bool IsMain { get; set; }
    public short SortOrder { get; set; }
}

/// <summary>Filtros para bÃºsqueda pÃºblica de atracciones.</summary>
public class AttractionSearchRequest
{
    public string? SearchTerm { get; set; }
    public Guid? LocationId { get; set; }
    public string? CategorySlug { get; set; }
    public Guid? SubcategoryId { get; set; }
    public Guid? TagId { get; set; }
    public short? LanguageId { get; set; }
    public decimal? MinRating { get; set; }
    public string? DifficultyLevel { get; set; }
    public string? DifficultyLevels { get; set; } // Comma separated
    public string? TagIds { get; set; } // Comma separated
    public string? LanguageIds { get; set; } // Comma separated
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}

/// <summary>PeticiÃ³n para crear una nueva atracciÃ³n.</summary>
public class CreateAttractionRequest
{
    [System.ComponentModel.DataAnnotations.Required]
    public string Name { get; set; } = string.Empty;
    [System.ComponentModel.DataAnnotations.Required]
    public Guid LocationId { get; set; }
    [System.ComponentModel.DataAnnotations.Required]
    public Guid SubcategoryId { get; set; }
    public string? DescriptionShort { get; set; }
    public string? DescriptionFull { get; set; }
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? MeetingPoint { get; set; }
    public string? DifficultyLevel { get; set; } // "easy" | "moderate" | "hard"
}

/// <summary>PeticiÃ³n para editar una atracciÃ³n existente.</summary>
public class UpdateAttractionRequest
{
    [System.ComponentModel.DataAnnotations.Required]
    public string Name { get; set; } = string.Empty;
    [System.ComponentModel.DataAnnotations.Required]
    public Guid LocationId { get; set; }
    [System.ComponentModel.DataAnnotations.Required]
    public Guid SubcategoryId { get; set; }
    public string? DescriptionShort { get; set; }
    public string? DescriptionFull { get; set; }
    public string? Address { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? MeetingPoint { get; set; }
    public string? DifficultyLevel { get; set; }
}

/// <summary>PeticiÃ³n para publicar/despublicar una atracciÃ³n.</summary>
public class ToggleAttractionStatusRequest
{
    [System.ComponentModel.DataAnnotations.Required]
    public bool IsPublished { get; set; }
}

/// <summary>PeticiÃ³n maestra para crear una atracciÃ³n con todo su ecosistema.</summary>
public class CreateCompleteAttractionRequest : CreateAttractionRequest
{
    public List<AttractionTranslationRequest> Translations { get; set; } = [];
    public List<AttractionGuideLanguageRequest> GuideLanguages { get; set; } = [];
    public List<AttractionMediaRequest> Media { get; set; } = [];
    public List<Guid> Tags { get; set; } = [];
    public List<AttractionInclusionRequest> Inclusions { get; set; } = [];
    public List<ProductOptionRequest> Products { get; set; } = [];
    public CompleteItineraryRequest? Itinerary { get; set; }
}

public class CompleteItineraryRequest
{
    public short LanguageId { get; set; } = 1; // Default a EspaÃ±ol (1)
    public string Overview { get; set; } = string.Empty;
    public List<CompleteTourStopRequest> Stops { get; set; } = [];
}

public class CompleteTourStopRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public short? StayTimeMinutes { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public short StopNumber { get; set; }
    public string AdmissionType { get; set; } = "included"; // 'included' | 'optional' | 'excluded' | 'bring'
}

public class AttractionInclusionRequest
{
    public Guid InclusionItemId { get; set; }
    public string Type { get; set; } = "included"; // 'included'|'not_included'|'optional'|'bring'
}

public class AttractionTranslationRequest
{
    public short LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? DescriptionShort { get; set; }
    public string? DescriptionFull { get; set; }
    public string? MeetingPoint { get; set; }
}

public class AttractionGuideLanguageRequest
{
    public short LanguageId { get; set; }
    public string GuideType { get; set; } = "live"; // 'live' | 'audio' | 'written' | 'app'
}

public class AttractionMediaRequest
{
    public short MediaTypeId { get; set; } // 1: image, 2: video
    public string Url { get; set; } = string.Empty;
    public string? Title { get; set; }
    public bool IsMain { get; set; }
    public short SortOrder { get; set; }
}

public class ProductOptionRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public string? DurationDescription { get; set; }
    public int CancelPolicyHours { get; set; } = 24;
    public string? CancelPolicyText { get; set; }
    public short? MaxGroupSize { get; set; }
    public short MinParticipants { get; set; } = 1;
    public bool IsPrivate { get; set; }
    public List<PriceTierRequest> PriceTiers { get; set; } = [];
}

public class PriceTierRequest
{
    public Guid TicketCategoryId { get; set; }
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; } = "USD";
}

