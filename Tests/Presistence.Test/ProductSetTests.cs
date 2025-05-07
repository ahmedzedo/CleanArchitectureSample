using System.Globalization;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Products.Entites;
using FluentAssertions;

namespace Presistence.Test
{
    public class ProductSetTests
    {
        private readonly IApplicationDbContext _context;
        public ProductSetTests()
        {
            var dbContextFactory = new ApplicationDbContextFactory();
            _context = dbContextFactory.DbContext;
        }
        private static Product AddNewProduct()
        {
            var product = new Product("منتج اول ", "product one", "product one");
            product.AddCategory(new Category("تصنيف اول",
                                             "Category one",
                                             "Category one",
                                             "تصنيف اول",
                                             "Category one",
                                             "Category one",
                                             DateTime.Parse("4/12/2023 4:05:48 PM", new CultureInfo("en-US")),
                                             true));
            product.AddProductItems([
                new ProductItem("New prduct item one", 50, 1000),
                new ProductItem("New prduct item two", 80, 5000)]);

            return product;
        }
        [Fact]
        public async Task AddAsync_AddNewProduct_ReturnAddProduct()
        {
            // Arrange
            Product product = AddNewProduct();

            // Act
            var result = await _context.Products.AddAsync(product);
            var count = await _context.SaveChangesAsync();
            var newProduct = await _context.Products.GetByIdAsync(product.Id);

            // Assert
            result.Id.Should().NotBeEmpty();
            count.Should().BeGreaterThan(1);
            newProduct.Should().NotBeNull();
            newProduct!.Id.Should().Be(product.Id);
        }
        [Fact]
        public async Task GetProductItemAsync_IfProdcutItemNotExist_ThrowNotFoundException()
        {
            // Arrange
            var getProductItem = () => _context.Products.GetProductItemAsync(Guid.NewGuid());
            // Act
            await Assert.ThrowsAsync<NotFoundException>(getProductItem);
            // Assert
        }
        [Fact]
        public async Task GetProductItemAsync_IfProductAdded_ReturnProductItem()
        {
            // Arrange
            var prodcut = AddNewProduct();
            await _context.Products.AddAsync(prodcut);
            await _context.SaveChangesAsync();

            // Act
            var productItem = await _context.Products.GetProductItemAsync(prodcut.ProductItems.First().Id);

            // Assert
            productItem.Id.Should().Be(prodcut.ProductItems.First().Id);
            productItem.ProductId.Should().Be(prodcut.Id);
        }
    }
}