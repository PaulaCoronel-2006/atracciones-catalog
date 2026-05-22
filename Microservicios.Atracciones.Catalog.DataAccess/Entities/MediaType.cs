namespace Microservicios.Atracciones.Catalog.DataAccess.Entities;

/// <summary>Lookup table: 1=image, 2=video, 3=document.</summary>
public class MediaType
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
