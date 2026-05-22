using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicios.Atracciones.Catalog.DataAccess.Entities;

public class PriceTierConfiguration : IEntityTypeConfiguration<PriceTier>
{
    public void Configure(EntityTypeBuilder<PriceTier> builder)
    {
        builder.ToTable("PriceTier", t => t.HasCheckConstraint("CK_PriceTier_Price", "price >= 0"));
    }
}
