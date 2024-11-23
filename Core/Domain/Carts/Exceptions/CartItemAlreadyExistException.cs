namespace CleanArchitecture.Domain.Carts.Exceptions
{
    public class CartItemAlreadyExistException : Exception
    {
        #region Constructor
        public CartItemAlreadyExistException(string Item)
          : base($"the {Item} cart item is already exist in cart")
        {
        }
        #endregion
    }
}
