using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Cart.Entities;
using CleanArchitecture.Domain.Product.Entites;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Carts.Services
{
    public class CartService : BaseService, ICartService
    {
        #region Dependencies
        protected IApplicationDbContext DbContext { get; }

        #endregion

        #region Constructors
        public CartService(IServiceProvider serviceProvider,
                           IApplicationDbContext dbContext) : base(serviceProvider)
        {
            DbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<Cart?> GetUserCartAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await DbContext.Carts//.IncludeItemDetails()
                                            .Include(i => i.Include(c => c.CartItems)
                                            .ThenInclude(ci => ci.ProductItem)
                                            .ThenInclude(pi => pi.Product)
                                            .ThenInclude(p => p != null ? p.Categories : null))
            //.Include(c => c.CartItems.Where(ci => ci.Count > 30))
            //.Include($"{nameof(CartItem)}s.{nameof(Product)}")
                                            .OrderByDescending(c => c.CreatedOn)
                                            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);


        }

        public async Task<Cart> AddOrUpdateCartItemAsync(Cart cart, Guid productItemId, int count, CancellationToken cancellationToken = default)
        {
            ProductItem productItem = await DbContext.Products.GetProductItemAsync(productItemId);

            var cartItem = cart.CartItems.FirstOrDefault(c => c.ProductItemId == productItem.Id);

            if (cartItem == null)
            {
                cart.AddCartItem(CartItem.CreateCartItem(cart, productItem, count));
            }
            else
            {
                cartItem.ChangeCount(count);
                DbContext.Carts.Update(cart);
            }

            return cart;
        }

        public async Task<Cart> AddUserCartAsync(Guid userId)
        {
            Cart cart = new(userId);

            return await DbContext.Carts.AddAsync(cart);
        }
        #endregion
    }
}
