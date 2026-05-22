using Microservicios.Atracciones.Catalog.DataAccess.Context;
using Microservicios.Atracciones.Catalog.DataAccess.Repositories.Interfaces;

namespace Microservicios.Atracciones.Catalog.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AtraccionDbContext _context;

    public UnitOfWork(AtraccionDbContext context)
    {
        _context = context;
    }

    // GeografÃ­a
    private ILocationRepository? _locations;
    public ILocationRepository Locations => _locations ??= new LocationRepository(_context);

    // Idiomas
    private ILanguageRepository? _languages;
    public ILanguageRepository Languages => _languages ??= new LanguageRepository(_context);

    // CatÃ¡logo
    private ICategoryRepository? _categories;
    private ISubcategoryRepository? _subcategories;
    private ITagRepository? _tags;
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
    public ISubcategoryRepository Subcategories => _subcategories ??= new SubcategoryRepository(_context);
    public ITagRepository Tags => _tags ??= new TagRepository(_context);

    // AtracciÃ³n
    private IAttractionRepository? _attractions;
    private IAudioGuideRepository? _audioGuides;
    private ITourItineraryRepository? _tourItineraries;
    public IAttractionRepository Attractions => _attractions ??= new AttractionRepository(_context);
    public IAudioGuideRepository AudioGuides => _audioGuides ??= new AudioGuideRepository(_context);
    public ITourItineraryRepository TourItineraries => _tourItineraries ??= new TourItineraryRepository(_context);

    // Inclusiones y Modalidades
    private IInclusionItemRepository? _inclusionItems;
    private IProductOptionRepository? _productOptions;
    public IInclusionItemRepository InclusionItems => _inclusionItems ??= new InclusionItemRepository(_context);
    public IProductOptionRepository ProductOptions => _productOptions ??= new ProductOptionRepository(_context);

    // Precios
    private IPriceTierRepository? _priceTiers;
    public IPriceTierRepository PriceTiers => _priceTiers ??= new PriceTierRepository(_context);

    // Otros
    private ITicketCategoryRepository? _ticketCategories;
    private ITourStopRepository? _tourStops;
    public ITicketCategoryRepository TicketCategories => _ticketCategories ??= new TicketCategoryRepository(_context);
    public ITourStopRepository TourStops => _tourStops ??= new TourStopRepository(_context);

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
