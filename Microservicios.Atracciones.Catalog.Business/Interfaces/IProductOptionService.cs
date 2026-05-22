using Microservicios.Atracciones.Catalog.Business.DTOs.Inventory;

namespace Microservicios.Atracciones.Catalog.Business.Interfaces;

public interface IProductOptionService
{
    Task<IEnumerable<ProductOptionDetailResponse>> GetByAttractionAsync(Guid attractionId);
    Task<ProductOptionDetailResponse> GetByIdAsync(Guid productId);
    Task<Guid> CreateAsync(CreateProductOptionRequest request);
    Task UpdateAsync(Guid productId, UpdateProductOptionRequest request);
    Task<bool> DeleteAsync(Guid productId);
    Task<bool> ToggleActiveAsync(Guid productId, bool isActive);
}

