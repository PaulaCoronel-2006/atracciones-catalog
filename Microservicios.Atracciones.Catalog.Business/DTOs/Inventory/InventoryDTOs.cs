namespace Microservicios.Atracciones.Catalog.Business.DTOs.Inventory;

/// <summary>Modalidad/producto de una atracciÃ³n con sus precios.</summary>
public class ProductResponse
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public string? DurationDescription { get; set; }
    public int CancelPolicyHours { get; set; }
    public string? CancelPolicyText { get; set; }
    public short? MaxGroupSize { get; set; }
    public short MinParticipants { get; set; }
    public bool IsPrivate { get; set; }
    public List<PriceTierResponse> PriceTiers { get; set; } = [];
}

public class PriceTierResponse
{
    public Guid Id { get; set; }
    public Guid TicketCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; } = "USD";
}

public class ProductOptionDetailResponse : ProductResponse
{
    public Guid AttractionId { get; set; }
    public bool IsActive { get; set; }
}

public class CreateProductOptionRequest
{
    public Guid AttractionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public string? DurationDescription { get; set; }
    public int CancelPolicyHours { get; set; } = 24;
    public string? CancelPolicyText { get; set; }
    public short? MaxGroupSize { get; set; }
    public short MinParticipants { get; set; } = 1;
    public bool IsPrivate { get; set; }
    public List<CreatePriceTierRequest> PriceTiers { get; set; } = [];
}

public class UpdateProductOptionRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public string? DurationDescription { get; set; }
    public int CancelPolicyHours { get; set; }
    public string? CancelPolicyText { get; set; }
    public short? MaxGroupSize { get; set; }
    public short MinParticipants { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsActive { get; set; }
}

public class CreatePriceTierRequest
{
    public Guid TicketCategoryId { get; set; }
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; } = "USD";
}

