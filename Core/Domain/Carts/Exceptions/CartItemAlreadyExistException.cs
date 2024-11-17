using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
