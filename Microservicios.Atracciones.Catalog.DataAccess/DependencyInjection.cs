using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microservicios.Atracciones.Catalog.DataAccess.Context;
using Microservicios.Atracciones.Catalog.DataAccess.Queries;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;

namespace Microservicios.Atracciones.Catalog.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        // â”€â”€ DbContext con conexiÃ³n Ãºnica â”€â”€
        services.AddDbContext<AtraccionDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        });

        // â”€â”€ Unit of Work â”€â”€
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // â”€â”€ Queries (Lectura Optimizada sin Tracking) â”€â”€
        services.AddScoped<IAttractionQueries, AttractionQueries>();

        // â”€â”€ Repositorio genÃ©rico â”€â”€
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}

