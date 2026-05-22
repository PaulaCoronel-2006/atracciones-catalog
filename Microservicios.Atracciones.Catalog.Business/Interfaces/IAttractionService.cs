using Microservicios.Atracciones.Catalog.Business.DTOs.Attraction;
using Microservicios.Atracciones.Catalog.Business.DTOs.Inventory;
using Microservicios.Atracciones.Catalog.DataAccess.Common;

namespace Microservicios.Atracciones.Catalog.Business.Interfaces;

public interface IAttractionService
{
    Task<PagedResult<AttractionSummaryResponse>> SearchAsync(AttractionSearchRequest request);
    Task<PagedResult<AttractionSummaryResponse>> SearchManagementAsync(AttractionSearchRequest request, Guid currentUserId, bool isAdmin);
    Task<AttractionDetailResponse?> GetDetailBySlugAsync(string slug, short? languageId = null);
    Task<IEnumerable<AttractionSummaryResponse>> GetTopRatedAsync(int count = 6);
    Task<IEnumerable<ProductResponse>> GetProductsAsync(Guid attractionId, short? languageId = null);
    Task<Guid> CreateAsync(CreateAttractionRequest request, Guid userId, bool isAdmin);
    Task<Guid> CreateCompleteAsync(CreateCompleteAttractionRequest request, Guid userId, bool isAdmin);
    Task<bool> UpdateAsync(Guid id, UpdateAttractionRequest request, Guid userId, bool isAdmin);
    Task<bool> DeleteAsync(Guid id, Guid userId, bool isAdmin);
    Task<bool> ToggleStatusAsync(Guid id, bool isPublished, Guid userId, bool isAdmin);
    Task<bool> ToggleActiveAsync(Guid id, bool isActive, Guid userId, bool isAdmin);
    Task<AttractionFullEditionResponse?> GetCompleteByIdAsync(Guid id, Guid userId, bool isAdmin);

    // Itinerarios
    Task<IEnumerable<ItineraryResponse>> GetItinerariesAsync(Guid attractionId);
    Task<Guid> CreateItineraryAsync(Guid attractionId, CreateItineraryRequest request);
    Task<Guid> AddStopAsync(Guid itineraryId, CreateTourStopRequest request);
    Task<bool> DeleteItineraryAsync(Guid id);
    Task<bool> DeleteStopAsync(Guid id);
}

