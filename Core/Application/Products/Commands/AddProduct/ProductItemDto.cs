using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Application.Products.Commands.AddProduct
{
    public record ProductItemDto
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
