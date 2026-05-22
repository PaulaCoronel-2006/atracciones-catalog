using Mapster;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using Microservicios.Atracciones.Catalog.DataManagement.Models;

namespace Microservicios.Atracciones.Catalog.DataManagement.Mappers;

public class InventoryMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductOption, ProductNode>()
            .Map(dest => dest.PriceTiers, src => src.PriceTiers);

        config.NewConfig<PriceTier, PriceTierNode>()
            .Map(dest => dest.CategoryName, src => src.TicketCategory != null ? src.TicketCategory.Name : string.Empty);
    }
}

