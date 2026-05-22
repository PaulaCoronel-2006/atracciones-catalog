using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;

namespace Microservicios.Atracciones.Catalog.DataAccess.Configurations;

// â”€â”€ Lookup Tables (con HasData seed) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Language");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedOnAdd();
        builder.HasIndex(l => l.IsoCode).IsUnique();
        builder.Property(l => l.IsoCode).HasMaxLength(5).IsRequired();
        builder.Property(l => l.Name).HasMaxLength(50).IsRequired();

        builder.HasData(
            new Language { Id = 1, IsoCode = "es", Name = "EspaÃ±ol" },
            new Language { Id = 2, IsoCode = "en", Name = "English" },
            new Language { Id = 3, IsoCode = "fr", Name = "FranÃ§ais" },
            new Language { Id = 4, IsoCode = "pt", Name = "PortuguÃªs" },
            new Language { Id = 5, IsoCode = "de", Name = "Deutsch" },
            new Language { Id = 6, IsoCode = "it", Name = "Italiano" },
            new Language { Id = 7, IsoCode = "zh", Name = "ä¸­æ–‡" },
            new Language { Id = 8, IsoCode = "ja", Name = "æ—¥æœ¬èªž" }
        );
    }
}

public class MediaTypeConfiguration : IEntityTypeConfiguration<MediaType>
{
    public void Configure(EntityTypeBuilder<MediaType> builder)
    {
        builder.ToTable("MediaType");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).HasMaxLength(20).IsRequired();
        builder.HasData(
            new MediaType { Id = 1, Name = "image" },
            new MediaType { Id = 2, Name = "video" },
            new MediaType { Id = 3, Name = "document" }
        );
    }
}

public class TicketCategoryConfiguration : IEntityTypeConfiguration<TicketCategory>
{
    public void Configure(EntityTypeBuilder<TicketCategory> builder)
    {
        builder.ToTable("TicketCategory");
        builder.HasKey(tc => tc.Id);
        builder.Property(tc => tc.Name).HasMaxLength(50).IsRequired();
        builder.Property(tc => tc.NameEn).HasMaxLength(50);

        builder.HasData(
            new TicketCategory { Id = Guid.Parse("A1B2C3D4-E5F6-4A1B-8C9D-0E1F2A3B4C5D"), Name = "Adulto", NameEn = "Adult", AgeRangeMin = 13, SortOrder = 1 },
            new TicketCategory { Id = Guid.Parse("B2C3D4E5-F6A1-4B2C-9D0E-1F2A3B4C5D6E"), Name = "NiÃ±o", NameEn = "Child", AgeRangeMin = 5, AgeRangeMax = 12, SortOrder = 2 },
            new TicketCategory { Id = Guid.Parse("C3D4E5F6-A1B2-4C3D-0E1F-2A3B4C5D6E7F"), Name = "BebÃ©", NameEn = "Infant", AgeRangeMax = 4, SortOrder = 3 },
            new TicketCategory { Id = Guid.Parse("D4E5F6A1-B2C3-4D4E-1F2A-3B4C5D6E7F8A"), Name = "Senior", NameEn = "Senior", AgeRangeMin = 65, SortOrder = 4 }
        );
    }
}

// â”€â”€ Tablas N-M â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

public class AttractionTagConfiguration : IEntityTypeConfiguration<AttractionTag>
{
    public void Configure(EntityTypeBuilder<AttractionTag> builder)
    {
        builder.ToTable("AttractionTag");
        builder.HasKey(at => new { at.AttractionId, at.TagId });
    }
}

public class AttractionInclusionConfiguration : IEntityTypeConfiguration<AttractionInclusion>
{
    public void Configure(EntityTypeBuilder<AttractionInclusion> builder)
    {
        builder.ToTable("AttractionInclusion", t => t.HasCheckConstraint("CK_AttrIncl_Type", "type IN ('included','not_included','optional','bring')"));
        builder.HasKey(ai => new { ai.AttractionId, ai.InclusionItemId });
        builder.Property(ai => ai.Type).HasMaxLength(20).IsRequired();
    }
}

// â”€â”€ Configuraciones simples â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

public class AttractionTranslationConfiguration : IEntityTypeConfiguration<AttractionTranslation>
{
    public void Configure(EntityTypeBuilder<AttractionTranslation> builder)
    {
        builder.ToTable("AttractionTranslation");
        builder.HasKey(t => t.Id);
        builder.HasIndex(t => new { t.AttractionId, t.LanguageId }).IsUnique();
        builder.Property(t => t.Name).HasMaxLength(150).IsRequired();
        builder.Property(t => t.DescriptionShort).HasMaxLength(255);
    }
}

public class AttractionLanguageConfiguration : IEntityTypeConfiguration<AttractionLanguage>
{
    public void Configure(EntityTypeBuilder<AttractionLanguage> builder)
    {
        builder.ToTable("AttractionLanguage", t => t.HasCheckConstraint("CK_AttrLang_GuideType",
            "guide_type IN ('live','audio','written','app')"));
        builder.HasKey(al => al.Id);
        builder.HasIndex(al => new { al.AttractionId, al.LanguageId, al.GuideType }).IsUnique();
        builder.Property(al => al.GuideType).HasMaxLength(20).IsRequired();
    }
}

public class AudioGuideConfiguration : IEntityTypeConfiguration<AudioGuide>
{
    public void Configure(EntityTypeBuilder<AudioGuide> builder)
    {
        builder.ToTable("AudioGuide");
        builder.HasKey(g => g.Id);
        builder.HasIndex(g => new { g.AttractionId, g.LanguageId }).IsUnique();
        builder.Property(g => g.Title).HasMaxLength(150).IsRequired();
        builder.Property(g => g.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(g => g.Stops)
               .WithOne(s => s.AudioGuide)
               .HasForeignKey(s => s.AudioGuideId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AudioGuideStopConfiguration : IEntityTypeConfiguration<AudioGuideStop>
{
    public void Configure(EntityTypeBuilder<AudioGuideStop> builder)
    {
        builder.ToTable("AudioGuideStop");
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => new { s.AudioGuideId, s.StopNumber }).IsUnique();
        builder.Property(s => s.Title).HasMaxLength(150).IsRequired();
        builder.Property(s => s.AudioUrl).IsRequired();
        builder.Property(s => s.Latitude).HasPrecision(9, 6);
        builder.Property(s => s.Longitude).HasPrecision(9, 6);
    }
}

public class TourItineraryConfiguration : IEntityTypeConfiguration<TourItinerary>
{
    public void Configure(EntityTypeBuilder<TourItinerary> builder)
    {
        builder.ToTable("TourItinerary");
        builder.HasKey(ti => ti.Id);
        builder.HasIndex(ti => new { ti.AttractionId, ti.LanguageId }).IsUnique();
        builder.Property(ti => ti.Title).HasMaxLength(150).IsRequired();
        builder.Property(ti => ti.TotalDistanceKm).HasPrecision(6, 2);
        builder.Property(ti => ti.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(ti => ti.Stops)
               .WithOne(s => s.Itinerary)
               .HasForeignKey(s => s.ItineraryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TourStopConfiguration : IEntityTypeConfiguration<TourStop>
{
    public void Configure(EntityTypeBuilder<TourStop> builder)
    {
        builder.ToTable("TourStop", t => t.HasCheckConstraint("CK_TourStop_Admission",
            "admission_type IS NULL OR admission_type IN ('included','optional','excluded','bring')"));
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => new { s.ItineraryId, s.StopNumber }).IsUnique();
        builder.Property(s => s.Name).HasMaxLength(150).IsRequired();
        builder.Property(s => s.AdmissionType).HasMaxLength(20);
        builder.Property(s => s.Latitude).HasPrecision(9, 6);
        builder.Property(s => s.Longitude).HasPrecision(9, 6);
    }
}

public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
{
    public void Configure(EntityTypeBuilder<ProductOption> builder)
    {
        builder.ToTable("ProductOption");
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => new { p.AttractionId, p.Slug }).IsUnique();
        builder.Property(p => p.Title).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Slug).HasMaxLength(150).IsRequired();
        builder.Property(p => p.DurationDescription).HasMaxLength(100);
        builder.Property(p => p.CancelPolicyHours).HasDefaultValue(24);
        builder.Property(p => p.MinParticipants).HasDefaultValue((short)1);
        builder.Property(p => p.IsActive).HasDefaultValue(true);
        builder.Property(p => p.IsPrivate).HasDefaultValue(false);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(p => p.Translations)
               .WithOne(t => t.Product)
               .HasForeignKey(t => t.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PriceTiers)
               .WithOne(pt => pt.ProductOption)
               .HasForeignKey(pt => pt.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

