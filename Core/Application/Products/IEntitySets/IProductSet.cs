using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Product.Entites;

namespace CleanArchitecture.Application.Products.IEntitySets
{
    public interface IProductSet : IEntitySet<Product>
    {
        Task<ProductItem> GetProductItemAsync(Guid productItemId);
    }
}
