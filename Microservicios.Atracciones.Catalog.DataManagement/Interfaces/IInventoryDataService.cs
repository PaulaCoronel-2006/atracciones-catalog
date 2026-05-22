using Microservicios.Atracciones.Catalog.DataManagement.Models;

namespace Microservicios.Atracciones.Catalog.DataManagement.Interfaces;

public interface IInventoryDataService
{
    // Obtener las modalidades de producto y sus respectivos precios
    Task<IEnumerable<ProductNode>> GetProductsAsync(Guid attractionId, short? languageId = null);

    // Obtener tiers de precio por sus IDs (para validar precios al reservar)
    Task<IEnumerable<PriceTierNode>> GetPriceTiersByIdsAsync(IEnumerable<Guid> tierIds);
}

