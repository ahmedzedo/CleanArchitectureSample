using CleanArchitecture.Application.Carts.IEntitySets;
using CleanArchitecture.Application.Categories.IEntitySets;
using CleanArchitecture.Application.Products.IEntitySets;
using CleanArchitecture.Application.Users.IEntitySet;

namespace CleanArchitecture.Application.Common.Abstracts.Persistence
{
    public interface IApplicationDbContext
    {
        IProductSet Products { get; }
        ICategorySet Categories { get; }
        ICartSet Carts { get; }
        IRefreshTokenSet RefreshTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        int SaveChanges();
    }
}
