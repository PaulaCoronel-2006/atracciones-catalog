namespace Microservicios.Atracciones.Catalog.DataAccess.Common;

/// <summary>
/// Resultado paginado genÃ©rico para cualquier consulta de listado.
/// </summary>
public class PagedResult<T> where T : class
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public PagedResult() { }

    public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int TotalPages => PageSize > 0
        ? (int)Math.Ceiling(TotalCount / (double)PageSize)
        : 0;

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>Crea un resultado vacÃ­o con la configuraciÃ³n de filtros dada.</summary>
    public static PagedResult<T> Empty(int pageNumber, int pageSize) => new()
    {
        Items = [],
        TotalCount = 0,
        PageNumber = pageNumber,
        PageSize = pageSize
    };
}
