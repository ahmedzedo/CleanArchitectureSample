using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Product.Entites;

namespace CleanArchitecture.Domain.Cart.Entities
{
    public class CartItem : AuditableEntity, IAggregateRoot
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
            if (cart is null)
            {
                throw new ArgumentNullException(nameof(cart));
            }
            else
            {
                if (productDetails is null)
                {
                    throw new ArgumentNullException(nameof(productDetails));
                }
                else
                {
                    return new CartItem
                    {
                        CartId = cart.Id,
                        ProductItemId = productDetails.Id,
                        ProductItem = productDetails,
                        Count = count < productDetails.Amount ? count : throw new ArgumentOutOfRangeException(nameof(count), $"{nameof(count)} more than amount")
                    };
                }
            }
        }
        public void ChangeCount(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Invaild item count");
            }
            Count = count;
        }
        #endregion

    }
}
