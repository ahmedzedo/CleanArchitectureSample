using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Domain.Cart.Entities;

namespace CleanArchitecture.Application.Carts.Services
{
    public interface ICartService : IService
    {
        Task<Cart> AddOrUpdateCartItemAsync(Cart cart, Guid productItemId, int count, CancellationToken cancellationToken = default);
        Task<Cart> AddUserCartAsync(Guid userId);
        Task<Cart?> GetUserCartAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
