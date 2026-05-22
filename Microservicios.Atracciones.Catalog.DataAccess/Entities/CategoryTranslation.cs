namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class CategoryTranslation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CategoryId { get; set; }
    public short LanguageId { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual Category Category { get; set; } = null!;
    public virtual Language Language { get; set; } = null!;
}

