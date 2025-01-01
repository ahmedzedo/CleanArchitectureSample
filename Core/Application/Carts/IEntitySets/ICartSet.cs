using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Application.Carts.IEntitySets
{
    public interface ICartSet : IEntitySet<Cart>
    {
        ICartSet IncludeItemDetails();

        Task<List<Cart>> GetCartsContainsProductItem(ProductItem productItem,
                                                     CancellationToken cancellationToken = default);
        Task<List<CartItem>> GetCartItemsOfProductItem(Guid productItemId,
                                                       CancellationToken cancellationToken = default);

        Task<int> DeleteCartItemsAsync(List<CartItem> cartItems,
                                       CancellationToken cancellationToken = default);
        Task<Cart?> GetTrackedUserCart(Guid userId, CancellationToken cancellationToken = default);

        Task<Cart?> GetUserCartAsync(Guid userId,
                                     CancellationToken cancellationToken = default);
        Task<Cart?> GetCartWithItem(Guid cartId,
                                    Guid? cartItemId = default,
                                    CancellationToken cancellationToken = default);
        Task<Cart?> GetCartByCartItemIdAsync(Guid cartItemId, CancellationToken cancellationToken);
    }

}
