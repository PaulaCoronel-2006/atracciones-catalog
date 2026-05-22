using Microservicios.Atracciones.Catalog.DataAccess.Common;

namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class Category : BaseEntity
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public short SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    // NavegaciÃ³n
    public virtual ICollection<Subcategory> Subcategories { get; set; } = [];
    public virtual ICollection<CategoryTranslation> Translations { get; set; } = [];
}
