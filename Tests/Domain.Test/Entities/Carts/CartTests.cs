using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Common.Exceptions;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Domain.Test.Entities.Cartss
{
    public class CartTests
    {
        [Fact]
        public void AddCartItem_ItemExistsInCart_ThrowArgumentNullException()
        {
            //Arrange
            Cart Cart = new(Guid.NewGuid().ToString());
            CartItem? cartItem = default;

            //Act
            Action<CartItem> addCartItemAction = (item) => Cart.AddCartItem(item);

            //Assert
            Assert.Throws<ArgumentNullException>(() => addCartItemAction(cartItem!));
        }
        [Fact]
        public void AddCartItem_ItemExistsInCart_ThrowArgumentAlreadyExistException()
        {
            //Arrange
            Cart Cart = new(Guid.NewGuid().ToString());
            var cartItem = CartItem.CreateCartItem(Cart, new ProductItem("product Item One", 150, 50)
            {
                Id = Guid.NewGuid()
            }, 10);

            Cart.AddCartItem(cartItem);

            //Act
            Action<CartItem> addCartItemAction = (item) => Cart.AddCartItem(item);

            //Assert
            Assert.Throws<ArgumentAlreadyExistException>(() => addCartItemAction(cartItem));
        }
        [Fact]
        public void AddCartItems_ItemsIsNull_ThrowNulltException()
        {
            //Arrange
            Cart Cart = new(Guid.NewGuid().ToString());
            List<CartItem>? cartItems = null!;

            //Act
            Action<List<CartItem>> addCartItemAction = (items) => Cart.AddCartItems(items);

            //Assert
            Assert.Throws<ArgumentNullException>(() => addCartItemAction(cartItems));
        }
        [Fact]
        public void AddCartItems_ItemsIsEmpty_ThrowArgumentEmptyListException()
        {
            //Arrange
            Cart Cart = new(Guid.NewGuid().ToString());
            List<CartItem>? cartItems = [];

            //Act
            Action<List<CartItem>> addCartItemAction = (items) => Cart.AddCartItems(items);

            //Assert
            Assert.Throws<ArgumentNullOrEmptyEnumerableException>(() => addCartItemAction(cartItems));
        }

        [Fact]
        public void UpdateCartItem_ItemsIsEmpty_ThrowArgumentEmptyListException()
        {
            //Arrange
            Cart Cart = new(Guid.NewGuid().ToString());
            List<CartItem>? cartItems = [];

            //Act
            Action<List<CartItem>> updateCartItemAction = (items) => Cart.UpdateCartItem(items);

            //Assert
            Assert.Throws<ArgumentNullOrEmptyEnumerableException>(() => updateCartItemAction(cartItems));
        }
        [Fact]
        public void RemoveCartItem_ItemsIsNotExist_ThrowArgumentNotExistException()
        {
            //Arrange
            Cart Cart = new(Guid.NewGuid().ToString());
            var productItem = new ProductItem("product Item One", 150, 50);
            productItem.Id = Guid.NewGuid();
            var cartItem = CartItem.CreateCartItem(Cart, productItem, 10);

            //Act
            Action<CartItem> removeCartItemAction = (cartItem) => Cart.RemoveCartItem(cartItem);

            //Assert
            Assert.Throws<ArgumentNotExistException>(() => removeCartItemAction(cartItem));
        }
        [Fact]
        public void RemoveCartItems_ItemsIsEmpty_ThrowArgumentEmptyListException()
        {
            //Arrange
            Cart Cart = new(Guid.NewGuid().ToString());
            List<CartItem>? cartItems = [];

            //Act
            Action<List<CartItem>> removeCartItemAction = (cartItems) => Cart.RemoveCartItems(cartItems);

            //Assert
            Assert.Throws<ArgumentNullOrEmptyEnumerableException>(() => removeCartItemAction(cartItems));
        }
    }
}