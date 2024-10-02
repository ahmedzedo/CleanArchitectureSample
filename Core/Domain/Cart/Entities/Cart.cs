using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Cart.Entities
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
            if (!cartItems.Contains(Item))
            {
                cartItems.Add(Item);
            }
            else { throw new ArgumentException("Already Exist"); }
        }
        public void AddCartItem(List<CartItem> Items)
        {
            if (cartItems.Count != 0)
            {
                cartItems.AddRange(Items.FindAll(i => !cartItems.Contains(i)));
            }
            else
            {
                throw new ArgumentException("Empty list");
            }
        }
        public void UpdateCartItem(List<CartItem> CartItemLst)
        {
            cartItems.Clear();
            cartItems.AddRange(CartItemLst);
        }
        public void RemoveCartItem(CartItem cartItem)
        {
            if (cartItems.Contains(cartItem))
            {
                cartItems.Remove(cartItem);
            }
            else
            {
                throw new ArgumentException("Not exist");
            }
        }
        public void RemoveCartItem(List<CartItem> CartItemLst)
        {
            cartItems.RemoveAll(r => CartItemLst.Contains(r));
        }
        #endregion

        #endregion
    }
}
