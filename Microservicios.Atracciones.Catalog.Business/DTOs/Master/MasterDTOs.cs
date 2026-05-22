namespace Microservicios.Atracciones.Catalog.Business.DTOs.Master;

public class CreateTagRequest
{
    public string Name { get; set; } = string.Empty;
}

public class CreateInclusionRequest
{
    public string DefaultText { get; set; } = string.Empty;
    public string? IconSlug { get; set; }
    public short LanguageId { get; set; } = 1;
}

public class LanguageResponse
{
    public short Id { get; set; }
    public string IsoCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class TicketCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public short? AgeRangeMin { get; set; }
    public short? AgeRangeMax { get; set; }
}

public class CreateTicketCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public short? AgeRangeMin { get; set; }
    public short? AgeRangeMax { get; set; }
    public short SortOrder { get; set; }
}

