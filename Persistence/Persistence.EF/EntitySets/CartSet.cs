using CleanArchitecture.Application.Carts.IEntitySets;
using CleanArchitecture.Domain.Cart.Entities;
using CleanArchitecture.Domain.Product.Entites;
using CleanArchitecture.Persistence.EF;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistence.EF.EntitySets
{
    public class CartSet : EntitySet<Cart>, ICartSet
    {
        #region Constructor
        public CartSet(ApplicationDbContext DbContext) : base(DbContext)
        {

        }
        #endregion

        #region Custom Methods

        public ICartSet IncludeItemDetails()
        {
            EntityQuery = EntityQuery.Include(c => c.CartItems.Where(ci => ci.Count > 30))
                             .ThenInclude(i => i.ProductItem)
                             .ThenInclude(p => p!.Product)
                             .ThenInclude(p => p!.Categories);

            return this;
        }

        public async Task<List<Cart>> GetCartsContainsProductItem(ProductItem productItem,
                                                                  CancellationToken cancellationToken = default)
        {
            return await Context.Set<Cart>()
                                   .Where(c => c.CartItems.Any(ci => ci.ProductItemId == productItem.Id))
                                   .Include(c => c.CartItems.Where(ci => ci.ProductItemId == productItem.Id))
                                   .ToListAsync(cancellationToken);

        }

        public async Task<int> DeleteCartItemsAsync(List<CartItem> cartItems, CancellationToken cancellationToken = default)
        {
            var lstIds = cartItems.Select(c => c.Id).ToList();
            return await Context.Set<CartItem>().Where(ci => lstIds.Contains(ci.Id)).ExecuteDeleteAsync(cancellationToken);
        }
        #endregion
    }
}
