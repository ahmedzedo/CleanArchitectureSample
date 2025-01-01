using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Products.Entites;
using MediatR;
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
        public async Task<Cart> AddOrUpdateUserCartAsync(Guid userId,
                                          Guid productItemId,
                                          int count,
                                          CancellationToken cancellationToken = default)
        {
            var cart = await DbContext.Carts.GetTrackedUserCart(userId, cancellationToken);

            cart ??= await AddUserCartAsync(userId);
            await AddOrUpdateCartItemAsync(cart, productItemId, count, cancellationToken);

            return cart;
        }
        public async Task<Cart?> GetUserCartAsync(Guid userId,
                                                  CancellationToken cancellationToken = default)
        {
            return await DbContext.Carts.GetUserCartAsync(userId, cancellationToken);


        }

        public async Task<Cart> AddOrUpdateCartItemAsync(Cart cart,
                                                         Guid productItemId,
                                                         int count,
                                                         CancellationToken cancellationToken = default)
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

        public async Task<Cart?> GetCartByIdIncludedItemById(Guid cartId,
                                                             Guid cartItemId,
                                                             CancellationToken cancellationToken = default)
        {
            return await DbContext.Carts.GetCartWithItem(cartId, cartItemId, cancellationToken);
        }

        public async Task<Cart?> GetCartByCartItemIdAsync(Guid cartItemId, CancellationToken cancellationToken)
        {
            return await DbContext.Carts.GetCartByCartItemIdAsync(cartItemId, cancellationToken);
        }

        public async Task<List<CartItem>> GetCartItemsOfProductItem(Guid productItemId,
                                                                    CancellationToken cancellationToken = default)
        {
            return await DbContext.Carts.GetCartItemsOfProductItem(productItemId, cancellationToken);
        }

        public async Task DeleteCartItems(List<CartItem> cartItems, CancellationToken cancellationToken = default)
        {
            await DbContext.Carts.DeleteCartItemsAsync(cartItems, cancellationToken);
        }
        #endregion
    }
}
