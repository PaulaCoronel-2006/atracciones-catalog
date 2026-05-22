namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class TourStopMedia
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StopId { get; set; }
    public short MediaTypeId { get; set; }
    public string Url { get; set; } = string.Empty;
    public short SortOrder { get; set; } = 0;

    public virtual TourStop Stop { get; set; } = null!;
    public virtual MediaType MediaType { get; set; } = null!;
}

