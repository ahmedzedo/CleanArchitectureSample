using System.Net;
using CleanArchitecture.Application.Carts.Commands.AddItemToCart;
using CleanArchitecture.Application.Common.Behaviours;
using CleanArchitecture.Application.Common.Messaging;
using FluentAssertions;
using Moq;

namespace Application.Test.Carts.Commands.AddItemToCart
{
    public class AddItemToCartCommandValidatorTests
    {
        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenProductItemIsEmpty()
        {
            // Arrange
            var command = new AddItemToCartCommand { ProductItemId = Guid.Empty, Count = 10 };  // Invalid because ProductItemId is empty
            var validator = new AddItemToCartCommandValidator();
            var behavior = new ValidationBehaviour<AddItemToCartCommand, Guid>([validator]);
            var next = new Mock<MyRequestHandlerDelegate<Guid>>();

            // Act
            var result = await behavior.Handle(command, next.Object, CancellationToken.None);

            // Assert
            // Ensure 'next' was never called since validation failed
            next.Verify(n => n(), Times.Never);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error?.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Error?.SubErrors?.Count.Should().Be(1);
            result.Error?.SubErrors?.Keys.First().Should().Be($"[{nameof(command.ProductItemId)}]:");
        }


        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCountIsLessThanOrEqualZero()
        {
            // Arrange
            var command = new AddItemToCartCommand { ProductItemId = Guid.NewGuid(), Count = -4 };  // Invalid because ProductItemId is empty
            var validator = new AddItemToCartCommandValidator();
            var behavior = new ValidationBehaviour<AddItemToCartCommand, Guid>(new[] { validator });
            var next = new Mock<MyRequestHandlerDelegate<Guid>>();

            // Act 
            var result = await behavior.Handle(command, next.Object, CancellationToken.None);

            // Assert
            // Ensure 'next' was never called since validation failed
            next.Verify(n => n(), Times.Never);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error?.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Error?.Message.Should().Be("Validation Error");
            result.Error?.SubErrors?.Count.Should().Be(1);
            result.Error?.SubErrors?.Keys.First().Should().Be($"[{nameof(command.Count)}]:");
        }
    }

}

