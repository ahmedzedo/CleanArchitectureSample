using CleanArchitecture.Domain.Product.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Persistence.EF.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.NameAr)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(e => e.NameEn)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(e => e.NameFr)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.HasMany(e => e.Categories)
                   .WithMany(e => e.AvailableProducts)
                   .UsingEntity(j => j.ToTable("CategoryProduct"));

            builder.HasMany(e => e.ProductItems)
                .WithOne(e => e.Product)
                .HasForeignKey(e => e.ProductId);



        }
    }
}
