using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Cart.Entities;
using CleanArchitecture.Domain.Product.Entites;

namespace CleanArchitecture.Application.Carts.IEntitySets
{
    public interface ICartSet : IEntitySet<Cart>
    {
        ICartSet IncludeItemDetails();

        Task<List<Cart>> GetCartsContainsProductItem(ProductItem productItem, CancellationToken cancellationToken = default);

        Task<int> DeleteCartItemsAsync(List<CartItem> cartItems, CancellationToken cancellationToken = default);
    }
}
