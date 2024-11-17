using CleanArchitecture.Domain.Products.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Persistence.EF.Configurations
{
    public class ProductItemConfiguration : IEntityTypeConfiguration<ProductItem>
    {
        public void Configure(EntityTypeBuilder<ProductItem> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Description)
                .HasMaxLength(250);

            builder.Property(e => e.Price)
                .HasColumnType("decimal")
            .HasPrecision(18, 2);

            builder.HasOne(e => e.Product)
                .WithMany(e => e.ProductItems)
                .HasForeignKey(e => e.ProductId);
        }
    }
}
