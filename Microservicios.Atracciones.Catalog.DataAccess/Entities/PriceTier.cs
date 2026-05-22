namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class PriceTier
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public Guid TicketCategoryId { get; set; }
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; } = "USD";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual ProductOption ProductOption { get; set; } = null!;
    public virtual TicketCategory TicketCategory { get; set; } = null!;
}
