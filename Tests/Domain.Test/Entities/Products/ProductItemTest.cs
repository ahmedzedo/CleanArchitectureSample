using CleanArchitecture.Domain.Products.Entites;
using CleanArchitecture.Domain.Products.Events;
using FluentAssertions;

namespace CleanArchitecture.Domain.Test.Entities.Products
{
    public class ProductItemTest
    {
        [Fact]
        public void ChangePrice_IfPriceLessThanZero_ThrowArgumentException()
        {
            // Arrange
            var productItem = new ProductItem("description", 150, 1223);
            var price = -1;

            // Act
            var changePriceFunc = () => productItem.ChangePrice(price);

            // Assert
            Assert.Throws<ArgumentException>(() => changePriceFunc());
        }

        [Fact]
        public void Update_WhenUpdatePriceAndAmount_ShouldPriceAndAmountUpdatedAndDomainEventsCreated()
        {
            // Arrange
            var productItem = new ProductItem("description", 150, 1223);
            var productItemToUpdate = new ProductItem("description", 333, 5555);

            // Act
            productItem.Update(productItemToUpdate);

            // Assert
            productItem.Price.Should().Be(productItemToUpdate.Price);
            productItem.Amount.Should().Be(productItemToUpdate.Amount);
            productItem.DomainEvents.Should().HaveCount(2);
            productItem.DomainEvents.Should().ContainItemsAssignableTo<ProductItemAmountChangedEvent>();
            productItem.DomainEvents.Should().ContainItemsAssignableTo<ProductItemPriceChangedEvent>();
        }

        [Fact]
        public void ChangeAmount_IfAmountLessThanZero_ThrowArgumentException()
        {
            // Arrange
            var productItem = new ProductItem("description", 150, 1223);
            var amount = -1;

            // Act
            var changeAmountFunc = () => productItem.ChangePrice(amount);

            // Assert
            Assert.Throws<ArgumentException>(() => changeAmountFunc());
        }
    }
}
