namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class SubcategoryTranslation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SubcategoryId { get; set; }
    public short LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual Subcategory Subcategory { get; set; } = null!;
    public virtual Language Language { get; set; } = null!;
}

