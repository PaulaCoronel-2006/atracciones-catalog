using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microservicios.Atracciones.Catalog.API.Protos;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;

namespace Microservicios.Atracciones.Catalog.API.Grpc;

public class CatalogGrpcService : CatalogGrpc.CatalogGrpcBase
{
    private readonly IUnitOfWork _uow;

    public CatalogGrpcService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public override async Task<ValidateProductResponse> ValidateProduct(ValidateProductRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.ProductId, out var productId))
        {
            return new ValidateProductResponse { IsValid = false };
        }

        var option = await _uow.ProductOptions.Query()
            .Include(po => po.PriceTiers)
            .FirstOrDefaultAsync(po => po.Id == productId && po.IsActive);

        if (option == null)
        {
            return new ValidateProductResponse { IsValid = false };
        }

        var basePrice = option.PriceTiers.FirstOrDefault(pt => pt.IsActive)?.Price ?? 0;

        return new ValidateProductResponse
        {
            IsValid = true,
            Name = option.Title,
            Price = (double)basePrice
        };
    }
}
