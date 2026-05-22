using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;

namespace Microservicios.Atracciones.Catalog.DataAccess.Configurations;

public class AttractionConfiguration : IEntityTypeConfiguration<Attraction>
{
    public void Configure(EntityTypeBuilder<Attraction> builder)
    {
        builder.ToTable("Attraction", t => t.HasCheckConstraint("CK_Attraction_Difficulty",
            "difficulty_level IN ('easy','moderate','hard')"));

        builder.HasKey(a => a.Id);
        builder.HasIndex(a => a.Slug).IsUnique();

        builder.Property(a => a.Slug).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Name).HasMaxLength(150).IsRequired();
        builder.Property(a => a.DescriptionShort).HasMaxLength(255);
        builder.Property(a => a.DifficultyLevel).HasMaxLength(20);
        builder.Property(a => a.Latitude).HasPrecision(9, 6);
        builder.Property(a => a.Longitude).HasPrecision(9, 6);
        builder.Property(a => a.RatingAverage).HasPrecision(3, 2).HasDefaultValue(0.00m);
        builder.Property(a => a.RatingCount).HasDefaultValue(0);
        builder.Property(a => a.IsPublished).HasDefaultValue(false);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(a => a.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        // Relaciones
        builder.HasOne(a => a.Location)
               .WithMany(l => l.Attractions)
               .HasForeignKey(a => a.LocationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Subcategory)
               .WithMany(s => s.Attractions)
               .HasForeignKey(a => a.SubcategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.ManagedById);

        builder.HasMany(a => a.Translations)
               .WithOne(t => t.Attraction)
               .HasForeignKey(t => t.AttractionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Languages)
               .WithOne(l => l.Attraction)
               .HasForeignKey(l => l.AttractionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Media)
               .WithOne(m => m.Attraction)
               .HasForeignKey(m => m.AttractionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.AudioGuides)
               .WithOne(g => g.Attraction)
               .HasForeignKey(g => g.AttractionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.Itineraries)
               .WithOne(i => i.Attraction)
               .HasForeignKey(i => i.AttractionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.ProductOptions)
               .WithOne(p => p.Attraction)
               .HasForeignKey(p => p.AttractionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
