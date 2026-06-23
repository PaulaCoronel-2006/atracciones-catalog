namespace Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // GeografÃ­a
    ILocationRepository Locations { get; }

    // Idiomas
    ILanguageRepository Languages { get; }

    // CatÃ¡logo
    ICategoryRepository Categories { get; }
    ISubcategoryRepository Subcategories { get; }
    ITagRepository Tags { get; }

    // AtracciÃ³n
    IAttractionRepository Attractions { get; }
    IAudioGuideRepository AudioGuides { get; }
    ITourItineraryRepository TourItineraries { get; }

    // Inclusiones y Modalidades
    IInclusionItemRepository InclusionItems { get; }
    IProductOptionRepository ProductOptions { get; }

    // Precios
    IPriceTierRepository PriceTiers { get; }

    // Otros
    ITicketCategoryRepository TicketCategories { get; }
    ITourStopRepository TourStops { get; }
    IGenericRepository<Entities.AvailabilitySlot> AvailabilitySlots { get; }

    Task<int> CompleteAsync();
}
