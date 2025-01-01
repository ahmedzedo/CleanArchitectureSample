using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Products.Queries.GetPagedProducts;
using CleanArchitecture.Domain.Products.Entites;
using CleanArchitecture.Domain.Products.Events;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Products.Services
{
    public class ProductService : BaseService, IProductService
    {
        #region Dependencies
        public IApplicationDbContext DbContext { get; }
        #endregion

        #region Constructor
        public ProductService(IServiceProvider serviceProvider,
                              IApplicationDbContext dbContext) : base(serviceProvider)
        {
            DbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<(IReadOnlyCollection<GetPagedProductDto> Items, int totalCount)> GetPagedProductsWithFilter(GetPagedProductsQuery request)
        {
            return await DbContext.Products
                   .Include(p => p.Include(s => s.ProductItems)
                                  .Include(p => p.Categories))
                   .WhereIf(!string.IsNullOrEmpty(request.Name), p => p.NameAr.Contains(request.Name!)
                                                                   || p.NameEn.Contains(request.Name!)
                                                                   || p.NameFr.Contains(request.Name!))
                   .WhereIf(!string.IsNullOrEmpty(request.Description), p => p.ProductItems
                                                                              .Any(pd => pd.Description!.Contains(request.Description!)))
                   .WhereIf(request.CategoriesIds != null && request.CategoriesIds.Count != 0, p => p.Categories.Select(r => r.Id)
                                                                                                .Any(r => request.CategoriesIds!.Contains(r)))
                   // .WhereIf(request.Price != default && request.Price > 0, p => p.ProductDetails.Any(pd => pd.Price == request.Price))

                   //.DynamicOrderBy(request.OrderByPropertyName ?? "CreatedOn", request.SortDirection!.ToLower() == "asc" ? SortDirection.Ascending : SortDirection.Descending)
                   .DynamicOrderBy(request.OrderProperties)
                   .ToPagedListAsync<GetPagedProductDto>(request.PageIndex, request.PageSize, p => p);
        }

        public async Task<Product> AddProduct(Product product,
                                              List<Category> categories,
                                              List<ProductItem> productItems,
                                              CancellationToken cancellationToken = default)
        {
            product.AddCategory(categories);
            product.AddProductItems(productItems);
            product.AddDomainEvent(new ProductCreatedEvent(product));

            return await DbContext.Products.AddAsync(product, cancellationToken);
        }
        #endregion
    }
}
