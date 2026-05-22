namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class ProductTranslation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public short LanguageId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DurationDescription { get; set; }
    public string? CancelPolicyText { get; set; }

    public virtual ProductOption Product { get; set; } = null!;
    public virtual Language Language { get; set; } = null!;
}

