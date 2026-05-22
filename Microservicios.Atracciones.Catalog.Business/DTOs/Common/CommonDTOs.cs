namespace Microservicios.Atracciones.Catalog.Business.DTOs.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
}

public class PagedApiResponse<T> : ApiResponse<IEnumerable<T>>
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

