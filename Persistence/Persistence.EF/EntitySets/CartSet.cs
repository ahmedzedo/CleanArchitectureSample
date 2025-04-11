using CleanArchitecture.Application.Carts.IEntitySets;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Products.Entites;
using CleanArchitecture.Infrastructure.Identity;
using Common.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;

namespace CleanArchitecture.Persistence.EF.EntitySets
{
    public class CartSet : EntitySet<Cart>, ICartSet
    {
        #region Constructor
        public CartSet(IDbContext DbContext) : base(DbContext)
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
        public async Task<Cart?> GetUserCartAsync(Guid userId,
                                                  CancellationToken cancellationToken = default)
        {
            return await GetUserCartQuery(userId).JoinUser();

        }

        public async Task<Cart?> JoinUser()
        {
            var lst = await EntityQuery.Join(Context.Set<ApplicationUser>(),
                 cart => cart.UserId,
                 user => user.Id,
                 (cart, user) => new
                 {
                     cart,
                     user
                 }).ToListAsync();

            return lst.Select(c =>
            {
                c.cart.User = c.user;
                return c.cart;
            }).FirstOrDefault();


        }

        public async Task<Cart?> GetCartWithItem(Guid cartId,
                                                 Guid? cartItemId = default,
                                                 CancellationToken cancellationToken = default)
        {
            return await DbSet.Include(c => c.CartItems.WhereIf(cartItemId != default, ci => ci.Id == cartItemId))
                              .AsTracking()
                              .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken);
        }
        public async Task<Cart?> GetCartByCartItemIdAsync(Guid cartItemId, CancellationToken cancellationToken)
        {
            return await DbSet.AsTracking()
                              .Include(c => c.CartItems.Where(ci => ci.Id == cartItemId))
                                  .ThenInclude(i => i.ProductItem)
                              .FirstOrDefaultAsync(c => c.CartItems.Any(ci => ci.Id == cartItemId),
                                                                             cancellationToken);
        }
        public async Task<List<Cart>> GetCartsContainsProductItem(ProductItem productItem,
                                                                  CancellationToken cancellationToken = default)
        {
            return await Context.Set<Cart>()
                                   .Where(c => c.CartItems.Any(ci => ci.ProductItemId == productItem.Id))
                                   .Include(c => c.CartItems.Where(ci => ci.ProductItemId == productItem.Id))
                                   .ToListAsync(cancellationToken);

        }
        public async Task<List<CartItem>> GetCartItemsOfProductItem(Guid productItemId,
                                                                 CancellationToken cancellationToken = default)
        {
            return await Context.Set<CartItem>()
                                .Where(ci => ci.ProductItem.Id == productItemId)
                                .ToListAsync(cancellationToken: cancellationToken);
        }
        public async Task<int> DeleteCartItemsAsync(List<CartItem> cartItems, CancellationToken cancellationToken = default)
        {
            var lstIds = cartItems.Select(c => c.Id).ToList();
            return await Context.Set<CartItem>().Where(ci => lstIds.Contains(ci.Id)).ExecuteDeleteAsync(cancellationToken);
        }
        public async Task<Cart?> GetTrackedUserCart(Guid userId, CancellationToken cancellationToken = default)
        {
            return await GetUserCartQuery(userId).AsTracking().FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        private CartSet GetUserCartQuery(Guid userId)
        {
            EntityQuery = EntityQuery.Where(c => c.UserId == userId.ToString()).Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.ProductItem)
                                     .ThenInclude(pi => pi.Product)
                                     .ThenInclude(p => p != null ? p.Categories : null)
                                            .OrderByDescending(c => c.CreatedOn);

            return this;
        }


        #endregion
    }
}
