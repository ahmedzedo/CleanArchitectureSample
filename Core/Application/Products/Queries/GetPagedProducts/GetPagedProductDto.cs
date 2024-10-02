using CleanArchitecture.Domain.Product.Entites;

namespace CleanArchitecture.Application.Products.Queries.GetPagedProducts
{
    public class GetPagedProductDto
    {
        #region Properties
        public Guid Id { get; init; }
        public string NameAr { get; init; } = string.Empty;
        public string NameEn { get; init; } = string.Empty;
        public string NameFr { get; init; } = string.Empty;

        public IReadOnlyCollection<CategoryDto> Categories { get; init; } = [];
        public IReadOnlyCollection<ProductItemDto> ProductItems { get; init; } = [];
        #endregion

        #region Mapping
        public static implicit operator GetPagedProductDto(Product product)
        {
            return new()
            {
                Id = product.Id,
                NameAr = product.NameAr,
                NameEn = product.NameEn,
                NameFr = product.NameFr,
                ProductItems = product.ProductItems.Select(p => (ProductItemDto)p).ToList(),
                Categories = product.Categories.Select(c => (CategoryDto)c).ToList(),
            };
        }


        #endregion

        public class CategoryDto
        {
            public Guid Id { get; init; }
            public string? CategoryNameAr { get; init; }
            public string? CategoryNameEn { get; init; }
            public string? CategoryNameFr { get; init; }

            public static implicit operator CategoryDto(Category category)
            {
                return new()
                {
                    Id = category.Id,
                    CategoryNameAr = category.NameAr,
                    CategoryNameEn = category.NameEn,
                    CategoryNameFr = category.NameFr
                };

            }
        }

        public class ProductItemDto
        {
            #region Properties
            public Guid Id { get; set; }
            public string? Description { get; init; }
            public Guid ProductId { get; set; }
            public decimal Price { get; init; }
            public int Amount { get; init; }
            #endregion

            #region Mapping 
            public static implicit operator ProductItem(ProductItemDto productDetailsDto)
            {
                var productDetails = new ProductItem(productDetailsDto.Description,
                                                     productDetailsDto.Price,
                                                     productDetailsDto.Amount)
                {
                    Id = productDetailsDto.Id
                };
                return productDetails;
            }

            public static implicit operator ProductItemDto(ProductItem productDetails)
            {
                return new ProductItemDto
                {
                    Id = productDetails.Id,
                    Description = productDetails.Description,
                    ProductId = productDetails.ProductId,
                    Price = productDetails.Price,
                    Amount = productDetails.Amount
                };
            }
            #endregion

        }
    }
}