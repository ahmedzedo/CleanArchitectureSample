using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Common.Exceptions;

namespace CleanArchitecture.Domain.Carts.Entities
{
    public class Cart : AuditableEntity, IAggregateRoot
    {
        #region Constructor
        public Cart(Guid userId)
        {
            cartItems = [];
            UserId = userId;
        }
        #endregion

        #region Properties
        public virtual Guid Id { get; }
        public Guid UserId { get; private set; }

        private readonly List<CartItem> cartItems;
        public virtual IReadOnlyCollection<CartItem> CartItems => cartItems.AsReadOnly();

        #endregion

        #region Methods

        #region Manage CartItems List
        public void AddCartItem(CartItem Item)
        {
            ArgumentNullException.ThrowIfNull(Item);

            if (CartItems.Contains(Item))
            {
                throw new ArgumentAlreadyExistException(Item.ProductItem?.Product?.NameEn ?? nameof(Item), nameof(Cart));
            }
            cartItems.Add(Item);
        }
        public void AddCartItems(List<CartItem> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);
            ArgumentNullOrEmptyEnumerableException.ThrowIfNullOrEmptyEnumerable(Items, $"{nameof(Items)} cart items");

            cartItems.AddRange(Items.FindAll(i => !cartItems.Contains(i)));
        }
        public void UpdateCartItem(List<CartItem> cartItemLst)
        {
            ArgumentNullOrEmptyEnumerableException.ThrowIfNullOrEmptyEnumerable(cartItemLst, $"{nameof(cartItemLst)} cart items");

            cartItems.Clear();
            cartItems.AddRange(cartItemLst);
        }
        public void RemoveCartItem(CartItem cartItem)
        {
            if (!cartItems.Remove(cartItem))
            {
                throw new ArgumentNotExistException(cartItem.ProductItem?.Product?.NameEn ?? nameof(cartItem), nameof(Cart));
            }
        }
        public void RemoveCartItems(List<CartItem> cartItemLst)
        {
            ArgumentNullException.ThrowIfNull(cartItemLst);
            ArgumentNullOrEmptyEnumerableException.ThrowIfNullOrEmptyEnumerable(cartItemLst, $"{nameof(cartItemLst)}");

            cartItems.RemoveAll(item => cartItemLst.Contains(item));
        }
        #endregion

        #endregion
    }
}
