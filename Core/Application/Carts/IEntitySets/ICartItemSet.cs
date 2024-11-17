using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Carts.Entities;

namespace CleanArchitecture.Application.Carts.IEntitySets
{
    public interface ICartItemSet : IEntitySet<CartItem>
    {
    }
}
