using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Domain.Carts.Entities;

namespace CleanArchitecture.Application.Carts.Services
{
    public interface ICartService : IBaseService
    {
        Task<Cart> AddOrUpdateUserCartAsync(Guid userId,
                                          Guid productItemId,
                                          int count,
                                          CancellationToken cancellationToken = default);
        Task<Cart> AddOrUpdateCartItemAsync(Cart cart,
                                            Guid productItemId,
                                            int count,
                                            CancellationToken cancellationToken = default);
        Task<Cart> AddUserCartAsync(Guid userId);
        Task<Cart?> GetUserCartAsync(Guid userId,
                                     CancellationToken cancellationToken = default);
        Task<Cart?> GetCartByIdIncludedItemById(Guid cartId,
                                                Guid cartItemId,
                                                CancellationToken cancellationToken = default);
        Task<Cart?> GetCartByCartItemIdAsync(Guid cartItemId, CancellationToken cancellationToken);

        Task<List<CartItem>> GetCartItemsOfProductItem(Guid productItemId,
                                                       CancellationToken cancellationToken = default);

        Task DeleteCartItems(List<CartItem> cartItems, CancellationToken cancellationToken = default);
    }
}
