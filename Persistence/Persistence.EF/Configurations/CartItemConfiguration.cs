using CleanArchitecture.Domain.Carts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Persistence.EF.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.Cart)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId);
            builder.HasOne(t => t.ProductItem);

        }
    }
}
