using Microservicios.Atracciones.Catalog.DataAccess.Common;

namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class Subcategory : BaseEntity
{
    public Guid CategoryId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public short SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    // NavegaciÃ³n
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<Attraction> Attractions { get; set; } = [];
    public virtual ICollection<SubcategoryTranslation> Translations { get; set; } = [];
}
