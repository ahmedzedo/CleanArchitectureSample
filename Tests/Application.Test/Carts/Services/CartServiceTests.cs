using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Products.IEntitySets;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Products.Entites;
using FluentAssertions;
using Moq;

namespace Application.Test.Carts.Services
{
    public class CartServiceTests
    {
        private readonly CartService cartService;
        private readonly Mock<IApplicationDbContext> applicationDbContextMock;
        private readonly Mock<IProductSet> productSetMock;
        public CartServiceTests()
        {
            productSetMock = new Mock<IProductSet>();
            applicationDbContextMock = new Mock<IApplicationDbContext>();
            applicationDbContextMock.Setup(d => d.Products).Returns(productSetMock.Object);
            cartService = new CartService(It.IsAny<IServiceProvider>(), applicationDbContextMock.Object);

        }

        [Fact]
        public async Task AddOrUpdateCartItemAsync_IfCartItemIsNull_ShouldNewAddItemToCart()
        {
            // Arrang 
            var cart = new Cart(Guid.NewGuid().ToString());
            var productItemId = Guid.NewGuid();
            var count = 10;
            var productItem = new ProductItem(It.IsAny<string>(), 150, 60)
            {
                Id = productItemId
            };
            applicationDbContextMock.Setup(d => d.Products.GetProductItemAsync(productItemId)).ReturnsAsync(productItem);
            // Act
            var result = await cartService.AddOrUpdateCartItemAsync(cart, productItemId, count, It.IsAny<CancellationToken>());
            // Assert
            result.CartItems.Should().NotBeNull();
            result.CartItems.Count.Should().Be(1);
            result.CartItems.First().Count.Should().Be(count);
            result.CartItems.First().ProductItemId.Should().Be(productItemId);

        }

        [Fact]
        public async Task AddOrUpdateCartItemAsync_IfCartItemIsNotNull_ShouldNewUpdateCartItem()
        {
            // Arrang 
            var productItemId = Guid.NewGuid();
            var productItem = new ProductItem(It.IsAny<string>(), 150, 60)
            {
                Id = productItemId
            };
            var cart = new Cart(Guid.NewGuid().ToString());
            cart.AddCartItem(CartItem.CreateCartItem(cart, productItem, 10));
            var newCount = 15;
            applicationDbContextMock.Setup(d => d.Products.GetProductItemAsync(productItemId)).ReturnsAsync(productItem);
            applicationDbContextMock.Setup(d => d.Carts.Update(cart));

            // Act
            var result = await cartService.AddOrUpdateCartItemAsync(cart, productItemId, newCount, It.IsAny<CancellationToken>());
            // Assert
            result.CartItems.Should().NotBeNull();
            result.CartItems.Count.Should().Be(1);
            result.CartItems.First().Count.Should().Be(newCount);
            result.CartItems.First().ProductItemId.Should().Be(productItemId);
        }
    }
}
