using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Domain.Test.Entities.Carts
{
    public class CartItemTest
    {
        [Fact]
        public void CreateCartItem_CartIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Cart? cart = null!;
            var productItem = new ProductItem("product Item One", 150, 50)
            {
                Id = Guid.NewGuid()
            };

            //Act
            Func<Cart, ProductItem, int, CartItem> createCartItem = (cart, productItem, count) => CartItem.CreateCartItem(cart, productItem, count);
            //Assert
            Assert.Throws<ArgumentNullException>(() => createCartItem(cart, productItem, 10));
        }

        [Fact]
        public void CreateCartItem_ProductItemIsNull_ThrowArgumentNullException()
        {
            //Arrange
            Cart cart = new(Guid.NewGuid().ToString());
            ProductItem? productItem = null!;

            //Act
            Func<Cart, ProductItem, int, CartItem> createCartItem = (cart, productItem, count) => CartItem.CreateCartItem(cart, productItem, count);
            //Assert
            Assert.Throws<ArgumentNullException>(() => createCartItem(cart, productItem, 10));
        }
        [Fact]
        public void CreateCartItem_ItemCountGreaterThanProductAmount_ThrowArgumentOutOfRangeException()
        {
            //Arrange
            Cart cart = new(Guid.NewGuid().ToString());
            var productItem = new ProductItem("product Item One", 150, 50)
            {
                Id = Guid.NewGuid()
            };
            var count = 60;
            //Act
            Func<Cart, ProductItem, int, CartItem> createCartItem = (cart, productItem, count) => CartItem.CreateCartItem(cart, productItem, count);
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => createCartItem(cart, productItem, count));
        }
        [Fact]
        public void CreateCartItem_ItemCountIfLessThanOrEqualZero_ThrowArgumentOutOfRangeException()
        {
            //Arrange
            Cart cart = new(Guid.NewGuid().ToString());
            var productItem = new ProductItem("product Item One", 150, 50)
            {
                Id = Guid.NewGuid()
            };
            var count = 0;
            //Act
            Func<Cart, ProductItem, int, CartItem> createCartItem = (cart, productItem, count) => CartItem.CreateCartItem(cart, productItem, count);
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => createCartItem(cart, productItem, count));
        }

        [Fact]
        public void ChangeCount_ItemCountIfLessThanOrEqualZero_ThrowArgumentOutOfRangeException()
        {
            //Arrange
            Cart cart = new(Guid.NewGuid().ToString());
            var productItem = new ProductItem("product Item One", 150, 50)
            {
                Id = Guid.NewGuid()
            };
            var count = 0;

            CartItem cartItem = CartItem.CreateCartItem(cart, productItem, 10);
            //Act
            Action<int> cartItemChangeCount = (count) => cartItem.ChangeCount(count);
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => cartItemChangeCount(count));
        }

        [Fact]
        public void ChangeCount_ItemCountGreaterThanProductAmount_ThrowArgumentOutOfRangeException()
        {
            //Arrange
            Cart cart = new(Guid.NewGuid().ToString());
            var productItem = new ProductItem("product Item One", 150, 50)
            {
                Id = Guid.NewGuid()
            };
            var count = 60;

            CartItem cartItem = CartItem.CreateCartItem(cart, productItem, 10);
            //Act
            Action<int> cartItemChangeCount = (count) => cartItem.ChangeCount(count);
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => cartItemChangeCount(count));
        }
    }
}
