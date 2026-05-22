namespace Microservicios.Atracciones.Catalog.DataManagement.Models;

public class ProductNode
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
    
    // Tiers de preicos asociados
    public List<PriceTierNode> PriceTiers { get; set; } = [];
}

public class PriceTierNode
{
    public Guid Id { get; set; }
    public Guid TicketCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; } = "USD";
}

