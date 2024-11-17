using CleanArchitecture.Application.Carts.IEntitySets;
using CleanArchitecture.Domain.Carts.Entities;

namespace CleanArchitecture.Persistence.EF.EntitySets
{
    public class CartItemSet : EntitySet<CartItem>, ICartItemSet
    {
        #region Constructor
        public CartItemSet(ApplicationDbContext DbContext) : base(DbContext)
        {

        }
        #endregion
    }
}
