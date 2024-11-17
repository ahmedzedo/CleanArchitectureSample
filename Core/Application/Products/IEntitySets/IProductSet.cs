using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Application.Products.IEntitySets
{
    public interface IProductSet : IEntitySet<Product>
    {
        Task<ProductItem> GetProductItemAsync(Guid productItemId);
    }
}
