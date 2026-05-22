using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;

namespace Microservicios.Atracciones.Catalog.DataAccess.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("Locations");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name).HasMaxLength(100).IsRequired();
        builder.Property(l => l.Type).HasMaxLength(50).IsRequired();
        builder.Property(l => l.CountryCode).HasMaxLength(2);
        
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(l => l.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        // RelaciÃ³n jerÃ¡rquica
        builder.HasOne(l => l.Parent)
               .WithMany(p => p.Children)
               .HasForeignKey(l => l.ParentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

