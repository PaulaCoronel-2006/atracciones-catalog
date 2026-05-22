using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Models;

namespace Microservicios.Atracciones.Catalog.DataManagement.Services;

public class InventoryDataService : IInventoryDataService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InventoryDataService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductNode>> GetProductsAsync(Guid attractionId, short? languageId = null)
    {
        var products = await _unitOfWork.ProductOptions.Query()
            .Include(p => p.PriceTiers.Where(pt => pt.IsActive))
                .ThenInclude(pt => pt.TicketCategory)
            .Include(p => p.Translations)
            .Where(p => p.AttractionId == attractionId && p.IsActive)
            .OrderBy(p => p.SortOrder)
            .ToListAsync();

        if (languageId.HasValue)
        {
            foreach (var product in products)
            {
                var translation = product.Translations.FirstOrDefault(t => t.LanguageId == languageId.Value);
                if (translation != null)
                {
                    product.Title = translation.Title;
                    product.Description = translation.Description ?? product.Description;
                    product.DurationDescription = translation.DurationDescription ?? product.DurationDescription;
                    product.CancelPolicyText = translation.CancelPolicyText ?? product.CancelPolicyText;
                }
            }
        }

        return _mapper.Map<IEnumerable<ProductNode>>(products);
    }

    public async Task<IEnumerable<PriceTierNode>> GetPriceTiersByIdsAsync(IEnumerable<Guid> tierIds)
    {
        var ids = tierIds.ToList();
        var tiers = await _unitOfWork.PriceTiers.Query()
            .Where(t => ids.Contains(t.Id) && t.IsActive)
            .ToListAsync();

        return _mapper.Map<IEnumerable<PriceTierNode>>(tiers);
    }
}

