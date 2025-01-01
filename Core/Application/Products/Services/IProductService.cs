using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Application.Products.Queries.GetPagedProducts;
using CleanArchitecture.Domain.Products.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Products.Services
{
    public interface IProductService : IBaseService
    {
        Task<Product> AddProduct(Product product, List<Category> categories, List<ProductItem> productItems, CancellationToken cancellationToken = default);
        Task<(IReadOnlyCollection<GetPagedProductDto> Items, int totalCount)> GetPagedProductsWithFilter(GetPagedProductsQuery request);
    }
}
