using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Domain.Carts.Entities
{
    public class CartItem : AuditableEntity
    {

        #region Consructor
        private CartItem()
        {

        }
        #endregion

        #region Properties
        public Guid Id { get; }
        public Guid ProductItemId { get; private set; }
        public Guid CartId { get; private set; }
        public virtual ProductItem ProductItem { get; private set; } = new ProductItem();
        public int Count { get; private set; }
        public virtual Cart? Cart { get; }
        #endregion

        #region Methods

        public static CartItem CreateCartItem(Cart cart, ProductItem productDetails, int count)
        {
            ArgumentNullException.ThrowIfNull(cart);

            ArgumentNullException.ThrowIfNull(productDetails);

            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(count, 0);

            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, productDetails.Amount);

            return new CartItem
            {
                CartId = cart.Id,
                ProductItemId = productDetails.Id,
                ProductItem = productDetails,
                Count = count
            };
        }
        public void ChangeCount(int count)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(count, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, ProductItem.Amount);

            Count = count;
        }
        #endregion

    }
}
