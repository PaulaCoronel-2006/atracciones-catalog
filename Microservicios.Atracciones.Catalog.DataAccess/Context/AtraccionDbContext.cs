using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microservicios.Atracciones.Catalog.DataAccess.Common;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;
using System.Text.Json;

namespace Microservicios.Atracciones.Catalog.DataAccess.Context;

public class AtraccionDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public AtraccionDbContext(
        DbContextOptions<AtraccionDbContext> options,
        IHttpContextAccessor? httpContextAccessor = null)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    // 1. MÃ“DULO GEOGRÃFICO
    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    public DbSet<Location> Locations { get; set; }

    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    // 2. IDIOMAS
    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    public DbSet<Language> Languages { get; set; }

    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    // 3. CATÃLOGO
    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
    public DbSet<Subcategory> Subcategories { get; set; }
    public DbSet<SubcategoryTranslation> SubcategoryTranslations { get; set; }
    public DbSet<Tag> Tags { get; set; }

    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    // 4. ATRACCIONES
    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    public DbSet<Attraction> Attractions { get; set; }
    public DbSet<AttractionTranslation> AttractionTranslations { get; set; }
    public DbSet<AttractionLanguage> AttractionLanguages { get; set; }
    public DbSet<AttractionTag> AttractionTags { get; set; }
    public DbSet<AttractionMedia> AttractionMedias { get; set; }
    public DbSet<AttractionInclusion> AttractionInclusions { get; set; }

    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    // 5. AUDIOGUÃA E ITINERARIO
    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    public DbSet<AudioGuide> AudioGuides { get; set; }
    public DbSet<AudioGuideStop> AudioGuideStops { get; set; }
    public DbSet<TourItinerary> TourItineraries { get; set; }
    public DbSet<TourStop> TourStops { get; set; }
    public DbSet<TourStopMedia> TourStopMedias { get; set; }

    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    // 6. INCLUSIONES
    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    public DbSet<InclusionItem> InclusionItems { get; set; }
    public DbSet<MediaType> MediaTypes { get; set; }

    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    // 7. INVENTARIO Y PRECIOS
    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    public DbSet<ProductOption> ProductOptions { get; set; }
    public DbSet<ProductTranslation> ProductTranslations { get; set; }
    public DbSet<PriceTier> PriceTiers { get; set; }
    public DbSet<TicketCategory> TicketCategories { get; set; }
    public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }

    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    // MODEL CREATING
    // â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•â•
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AtraccionDbContext).Assembly);

        // Mapeo manual para resolver la inconsistencia de nombres en tu BD
        var tableMapping = new Dictionary<string, string>
        {
            { nameof(Location), "locations" },
            { nameof(Category), "category" },
            { nameof(CategoryTranslation), "category_translation" },
            { nameof(Language), "language" },
            { nameof(Subcategory), "subcategory" },
            { nameof(SubcategoryTranslation), "subcategory_translation" },
            { nameof(InclusionItem), "inclusion_item" },
            { nameof(Attraction), "attraction" },
            { nameof(AttractionTranslation), "attraction_translation" },
            { nameof(AttractionTag), "attraction_tag" },
            { nameof(Tag), "tag" },
            { nameof(AttractionInclusion), "attraction_inclusion" },
            { nameof(AttractionLanguage), "attraction_language" },
            { nameof(AttractionMedia), "attraction_media" },
            { nameof(MediaType), "media_type" },
            { nameof(ProductOption), "product_option" },
            { nameof(ProductTranslation), "product_translation" },
            { nameof(PriceTier), "price_tier" },
            { nameof(TicketCategory), "ticket_category" },
            { nameof(AvailabilitySlot), "availability_slot" },
            { nameof(TourItinerary), "tour_itinerary" },
            { nameof(TourStop), "tour_stop" },
            { nameof(TourStopMedia), "tour_stop_media" },
            { nameof(AudioGuide), "audio_guide" },
            { nameof(AudioGuideStop), "audio_guide_stop" }
        };

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var entityName = entity.ClrType.Name;
            if (tableMapping.TryGetValue(entityName, out var tableName))
            {
                entity.SetTableName(tableName);
            }

            foreach (var property in entity.GetProperties())
            {
                var propName = ToSnakeCase(property.Name);
                property.SetColumnName(propName);
            }
        }
    }

    private string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return System.Text.RegularExpressions.Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void StampAuditFields()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Property(e => e.CreatedAt).IsModified = false;
                    break;
            }
        }
    }
}
