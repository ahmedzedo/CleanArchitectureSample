using AutoMapper;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Application.Carts.Queries.GetCart
{
    public record CartDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public IReadOnlyCollection<CartItemDto> CartItems { get; init; } = [];

        #region Auto Mapping
        public sealed class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Cart, CartDto>();
                CreateMap<CartItem, CartItemDto>()
                    .ForMember(d => d.ProductNameAr, opt => opt.MapFrom(s => s.ProductItem.Product != null ? s.ProductItem.Product.NameAr : ""))
                    .ForMember(d => d.ProductNameEn, opt => opt.MapFrom(s => s.ProductItem.Product != null ? s.ProductItem.Product.NameEn : ""))
                    .ForMember(d => d.ProductNameFr, opt => opt.MapFrom(s => s.ProductItem.Product != null ? s.ProductItem.Product.NameFr : ""))
                    .ForMember(d => d.Amount, opt => opt.MapFrom(s => s.ProductItem != null ? s.ProductItem.Amount : 0))
                    .ForMember(d => d.Price, opt => opt.MapFrom(s => s.ProductItem != null ? s.ProductItem.Price : 0))
                    .ForMember(d => d.Categories, opt => opt.MapFrom(s => s.ProductItem.Product != null ? s.ProductItem.Product.Categories : new List<Category>()));

                CreateMap<Category, CategoryDto>()
                    .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.Id))
                    .ForMember(d => d.CategoryNameAr, opt => opt.MapFrom(s => s.NameAr))
                    .ForMember(d => d.CategoryNameEn, opt => opt.MapFrom(s => s.NameEn))
                    .ForMember(d => d.CategoryNameFr, opt => opt.MapFrom(s => s.NameFr));
            }
        }
        #endregion

        public static implicit operator CartDto(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                CartItems = cart.CartItems.Select(x => (CartItemDto)x).ToList()
            };
        }
    }
    public record CartItemDto
    {
        public Guid Id { get; init; }
        public Guid ProductItemId { get; init; }
        public string ProductNameAr { get; init; } = string.Empty;
        public string ProductNameEn { get; init; } = string.Empty;
        public string ProductNameFr { get; init; } = string.Empty;
        public IReadOnlyCollection<CategoryDto> Categories { get; init; } = [];

        public decimal Price { get; init; }
        public int Amount { get; set; }
        public int Count { get; set; }

        public static implicit operator CartItemDto(CartItem cartItem)
        {
            return new CartItemDto
            {
                Id = cartItem.CartId,
                Amount = cartItem.ProductItem?.Amount ?? default,
                Count = cartItem.Count,
                Price = cartItem.ProductItem?.Price ?? default,
                ProductItemId = cartItem.ProductItem?.ProductId ?? Guid.Empty,
                ProductNameAr = cartItem.ProductItem?.Product?.NameAr ?? string.Empty,
                ProductNameEn = cartItem.ProductItem?.Product?.NameEn ?? string.Empty,
                ProductNameFr = cartItem.ProductItem?.Product?.NameFr ?? string.Empty,
                Categories = cartItem.ProductItem?.Product?.Categories?.Select(x => (CategoryDto)x).ToList() ?? []
            };
        }
    }
    public record CategoryDto
    {
        public Guid CategoryId { get; init; }
        public string CategoryNameAr { get; init; } = string.Empty;
        public string CategoryNameEn { get; init; } = string.Empty;
        public string CategoryNameFr { get; init; } = string.Empty;

        public static implicit operator CategoryDto(Category category)
        {
            return new CategoryDto
            {
                CategoryId = category.Id,
                CategoryNameAr = category.NameAr,
                CategoryNameEn = category.NameEn,
                CategoryNameFr = category.NameFr,
            };
        }
    }
}









































