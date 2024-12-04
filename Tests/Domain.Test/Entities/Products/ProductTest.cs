using CleanArchitecture.Domain.Products.Entites;
using CleanArchitecture.Domain.Products.Events;
using FluentAssertions;

namespace CleanArchitecture.Domain.Test.Entities.Products
{
    public class ProductTest
    {
        #region Update Test
        [Fact]
        public void Update_NameArIsEmpty_ThrowArgumentException()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");

            // Act
            var updateFunc = () => product.Update("", "Product 1", "Product 1");
            // Assert

            Assert.Throws<ArgumentException>(updateFunc);

        }
        [Fact]
        public void Update_NameEnIsEmpty_ThrowArgumentException()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");

            // Act
            var updateFunc = () => product.Update("1 منتج ", "", "Product 1");
            // Assert

            Assert.Throws<ArgumentException>(updateFunc);

        }

        [Fact]
        public void Update_NameFrIsEmpty_ThrowArgumentException()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");

            // Act
            var updateFunc = () => product.Update("1 منتج ", "Product 1", "");
            // Assert

            Assert.Throws<ArgumentException>(updateFunc);

        }
        #endregion

        #region Add Category Test
        [Fact]
        public void AddCategory_WhenAddCategory_ShouldNewCategoryAddedToProductCategory()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");
            var category = new Category("تصنيف 1", "Product 1", "Product 1", "Product 1", "Product 1", "Product 1", DateTime.Now, true);
            // Act
            product.AddCategory(category);
            // Assert
            product.Categories.Count.Should().Be(1);
            product.Categories.First().Should().BeEquivalentTo(category);
        }

        [Fact]
        public void AddCategory_WhenAddCategoryList_ShouldCategoriesContainsAddedList()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");
            List<Category> categories =
                [new Category("تصنيف 1", "Product 1", "Product 1", "Product 1", "Product 1", "Product 1", DateTime.Now, true),
                 new Category("تصنيف 2", "Product 2", "Product 2", "Product 2", "Product 2", "Product 2", DateTime.Now, true),
                 new Category("تصنيف 3", "Product 3", "Product 3", "Product 3", "Product 3", "Product 3", DateTime.Now, true)];
            // Act
            product.AddCategory(categories);
            // Assert
            product.Categories.Count.Should().Be(3);
            product.Categories.AsEnumerable().Should().BeEquivalentTo(categories.AsEnumerable());
        }
        #endregion

        #region Update Category Test
        [Fact]
        public void UpdateCategory_WhenUpdateCategoryList_ShouldCategoriesContainsUpdatedList()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");
            List<Category> categories =
                [new Category("تصنيف 1", "Product 1", "Product 1", "Product 1", "Product 1", "Product 1", DateTime.Now, true),
                 new Category("تصنيف 2", "Product 2", "Product 2", "Product 2", "Product 2", "Product 2", DateTime.Now, true),
                 new Category("تصنيف 3", "Product 3", "Product 3", "Product 3", "Product 3", "Product 3", DateTime.Now, true)];
            product.AddCategory(categories);

            List<Category> newCategories =
               [new Category("تصنيف 100", "Product 100", "Product 100", "Product 100", "Product 100", "Product 100", DateTime.Now, true),
                new Category("تصنيف 200", "Product 200", "Product 200", "Product 200", "Product 200", "Product 200", DateTime.Now, true)];

            // Act
            product.UpdateCategory(newCategories);
            // Assert
            product.Categories.Count.Should().Be(2);
            product.Categories.AsEnumerable().Should().BeEquivalentTo(newCategories.AsEnumerable());
        }
        #endregion

        #region Remove Category Test
        [Fact]
        public void RemoveCategory_WhenRemoveCategory_ShouldCategoriesNotContainsTheRemoved()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");
            List<Category> categories =
                [new Category("تصنيف 1", "Product 1", "Product 1", "Product 1", "Product 1", "Product 1", DateTime.Now, true),
                 new Category("تصنيف 2", "Product 2", "Product 2", "Product 2", "Product 2", "Product 2", DateTime.Now, true),
                 new Category("تصنيف 3", "Product 3", "Product 3", "Product 3", "Product 3", "Product 3", DateTime.Now, true)];
            product.AddCategory(categories);

            // Act
            product.RemoveCategory(categories[2]);
            // Assert
            product.Categories.Count.Should().Be(2);
            product.Categories.Should().NotContainEquivalentOf(categories[2]);
        }

        [Fact]
        public void RemoveCategory_WhenRemoveCategoryList_ShouldCategoriesNotContainsRemovedList()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");
            List<Category> categories =
                [new Category("تصنيف 1", "Product 1", "Product 1", "Product 1", "Product 1", "Product 1", DateTime.Now, true),
                 new Category("تصنيف 2", "Product 2", "Product 2", "Product 2", "Product 2", "Product 2", DateTime.Now, true),
                 new Category("تصنيف 3", "Product 3", "Product 3", "Product 3", "Product 3", "Product 3", DateTime.Now, true)];
            product.AddCategory(categories);

            // Act
            product.RemoveCategory(categories);
            // Assert
            product.Categories.Count.Should().Be(0);
        }
        #endregion

        #region Add ProductItem Test
        [Fact]
        public void AddProductItem_WhenAddProductItem_ShouldNewProductItemAddedToProductProductItems()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");
            var productItem = new ProductItem("Test prodcut Item", 150, 1000);

            // Act
            product.AddProductItem(productItem);

            // Assert
            product.ProductItems.Count.Should().Be(1);
            product.ProductItems.First().Should().BeEquivalentTo(productItem);
        }

        [Fact]
        public void AddProductItem_WhenAddProductItemList_ShouldProductItemsContainsAddedList()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");
            List<ProductItem> productItems =
                [new ProductItem("Test prodcut 1 Item", 254, 1000),
                 new ProductItem("Test prodcut 2 Item", 451, 1000),
                 new ProductItem("Test prodcut 3 Item", 652, 1000)];

            // Act
            product.AddProductItems(productItems);

            // Assert
            product.ProductItems.Count.Should().Be(3);
            product.ProductItems.AsEnumerable().Should().BeEquivalentTo(productItems.AsEnumerable());
        }
        #endregion

        #region Update ProductItem Test
        [Fact]
        public void UpdateProductItem_WhenUpdateProductItemList_ShouldProductItemsContainsUpdatedList()
        {
            // Arrange
            var itemId1 = Guid.NewGuid();
            var itemId2 = Guid.NewGuid();
            var itemId3 = Guid.NewGuid();

            var product = new Product("منتج 1", "Product 1", "Product 1");
            List<ProductItem> productItems =
               [new ProductItem("Test prodcut 1 Item", 254, 1000),
                 new ProductItem("Test prodcut 2 Item", 451, 2586),
                 new ProductItem("Test prodcut 3 Item", 652, 9631)];
            productItems[0].Id = itemId1;
            productItems[1].Id = itemId2;
            productItems[2].Id = itemId3;


            product.AddProductItems(productItems);

            List<ProductItem> newProductItems =
                [new ProductItem("Test prodcut 100 Item", 356, 5000),
                 new ProductItem("Test prodcut 200 Item", 366, 36646)];
            newProductItems[0].Id = itemId1;
            newProductItems[1].Id = Guid.Empty;

            // Act
            product.UpdateProductItems(newProductItems);
            // Assert
            product.ProductItems.Should().HaveCount(2);
            product.ProductItems.Should().NotContain([productItems[1], productItems[2]]);
            product.ProductItems.ElementAt(0).Id.Should().Be(itemId1);
            product.ProductItems.ElementAt(0).Price.Should().Be(356);
            product.ProductItems.ElementAt(0).Amount.Should().Be(5000);
            product.ProductItems.ElementAt(0).DomainEvents.Should().HaveCount(2);
            product.ProductItems.ElementAt(1).Should().BeEquivalentTo(newProductItems[1]);
        }
        #endregion

        #region Remove ProductItem Test
        [Fact]
        public void RemoveProductItem_WhenRemoveProductItem_ShouldProductItemsNotContainsTheRemoved()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");
            List<ProductItem> productItems =
               [new ProductItem("Test prodcut 1 Item", 254, 1000),
                 new ProductItem("Test prodcut 2 Item", 451, 2586),
                 new ProductItem("Test prodcut 3 Item", 652, 9631)];

            product.AddProductItems(productItems);

            // Act
            product.RemoveProductItem(productItems[2]);
            // Assert
            product.ProductItems.Count.Should().Be(2);
            product.ProductItems.Should().NotContainEquivalentOf(productItems[2]);
            product.DomainEvents.Count.Should().Be(1);

        }

        [Fact]
        public void RemoveProductItem_WhenRemoveProductItemList_ShouldProductItemsNotContainsRemovedList()
        {
            // Arrange
            var product = new Product("منتج 1", "Product 1", "Product 1");
            List<ProductItem> productItems =
                [new ProductItem("Test prodcut 1 Item", 254, 1000),
                 new ProductItem("Test prodcut 2 Item", 451, 2586),
                 new ProductItem("Test prodcut 3 Item", 652, 9631)];

            product.AddProductItems(productItems);

            // Act
            product.RemoveProductItems([productItems[0], productItems[1]]);

            // Assert
            product.ProductItems.Count.Should().Be(1);
            product.DomainEvents.Count.Should().Be(1);
            product.DomainEvents.ElementAt(0).Should().BeAssignableTo<ProductItemsDeletedEvent>();
            (product.DomainEvents.ElementAt(0) as ProductItemsDeletedEvent).Should().NotBeNull();
            (product.DomainEvents.ElementAt(0) as ProductItemsDeletedEvent)!.ProductItems.Should().BeEquivalentTo([productItems[0], productItems[1]]);
        }
        #endregion


    }
}
