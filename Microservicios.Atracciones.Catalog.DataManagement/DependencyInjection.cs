using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microservicios.Atracciones.Catalog.DataManagement.Interfaces;
using Microservicios.Atracciones.Catalog.DataManagement.Services;
using System.Reflection;

namespace Microservicios.Atracciones.Catalog.DataManagement;

public static class DependencyInjection
{
    public static IServiceCollection AddDataManagementServices(this IServiceCollection services)
    {
        // 1. Configurar Mapster
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
        
        services.AddSingleton(typeAdapterConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        // 2. Registrar Data Services
        services.AddScoped<IAttractionDataService, AttractionDataService>();
        services.AddScoped<IInventoryDataService, InventoryDataService>();
        services.AddScoped<ILocationDataService, LocationDataService>();
        services.AddScoped<ICategoryDataService, CategoryDataService>();
        services.AddScoped<IMasterDataDataService, MasterDataDataService>();

        return services;
    }
}

