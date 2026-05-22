using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microservicios.Atracciones.Catalog.Business.Interfaces;
using Microservicios.Atracciones.Catalog.Business.Services;


namespace Microservicios.Atracciones.Catalog.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // â”€â”€â”€ Servicios de negocio â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        services.AddScoped<IAttractionService, AttractionService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMasterDataService, MasterDataService>();
        services.AddScoped<IProductOptionService, ProductOptionService>();
        services.AddScoped<IStorageService, LocalStorageService>();

        // â”€â”€â”€ Validadores FluentValidation â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        // Se pueden aÃ±adir validadores especÃ­ficos del catÃ¡logo aquÃ­ si existen

        return services;
    }
}

