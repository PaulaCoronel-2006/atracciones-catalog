using Microservicios.Atracciones.Catalog.Business.DTOs.Inventory;

namespace Microservicios.Atracciones.Catalog.Business.DTOs.Attraction;

// â”€â”€ Requests â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

/// <summary>Crea una nueva modalidad (ProductOption) bajo una atracciÃ³n existente.</summary>
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
    public bool IsPrivate { get; set; } = false;
    public List<PriceTierRequest> PriceTiers { get; set; } = [];
}

/// <summary>Edita los datos generales de una modalidad existente.</summary>
public class UpdateProductOptionRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? DurationMinutes { get; set; }
    public string? DurationDescription { get; set; }
    public int CancelPolicyHours { get; set; } = 24;
    public string? CancelPolicyText { get; set; }
    public short? MaxGroupSize { get; set; }
    public short MinParticipants { get; set; } = 1;
    public bool IsPrivate { get; set; } = false;
    public bool IsActive { get; set; } = true;
}

// â”€â”€ Responses â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

public class ProductOptionDetailResponse
{
    public Guid Id { get; set; }
    public Guid AttractionId { get; set; }
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
    public bool IsActive { get; set; }
    public List<PriceTierResponse> PriceTiers { get; set; } = [];
}

