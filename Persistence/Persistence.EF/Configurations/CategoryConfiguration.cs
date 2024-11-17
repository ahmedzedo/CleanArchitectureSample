using CleanArchitecture.Domain.Products.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Persistence.EF.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
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

            builder.Property(e => e.BriefAr)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(e => e.BriefEn)
                  .HasMaxLength(50)
                  .IsRequired();

            builder.Property(e => e.BriefFr)
                  .HasMaxLength(50)
                  .IsRequired();

            builder.Property(e => e.IsAvailable)
                   .HasDefaultValueSql("0");

            builder.Property(e => e.ApplyingDate).HasColumnType("DateTime");

            builder.HasMany(p => p.AvailableProducts)
                  .WithMany(p => p.Categories)
                  .UsingEntity(j => j.ToTable("CategoryProduct"));


        }
    }
}
