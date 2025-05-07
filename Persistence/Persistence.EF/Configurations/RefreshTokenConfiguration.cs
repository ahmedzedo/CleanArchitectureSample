using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Persistence.EF.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken", "Identity");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Token)
                   .HasMaxLength(200);
            builder.HasIndex(r => r.Token)
                   .IsUnique();

        }
    }
}
