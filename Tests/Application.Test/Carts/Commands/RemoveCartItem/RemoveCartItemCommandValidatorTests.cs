using System.Net;
using CleanArchitecture.Application.Carts.Commands.RemoveCartItem;
using CleanArchitecture.Application.Common.Behaviours;
using CleanArchitecture.Application.Common.Messaging;
using FluentAssertions;
using Moq;

namespace Application.Test.Carts.Commands.RemoveCartItem
{
    public class RemoveCartItemCommandValidatorTests
    {
        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCartIdIsEmpty()
        {
            // Arrange
            var command = new RemoveCartItemCommand { CartId = Guid.Empty, CartItemId = Guid.NewGuid() };  // Invalid because ProductItemId is empty
            var validator = new RemoveCartItemCommandValidator();
            var behavior = new ValidationBehaviour<RemoveCartItemCommand, bool>([validator]);
            var next = new Mock<MyRequestHandlerDelegate<bool>>();

            // Act
            var result = await behavior.Handle(command, next.Object, CancellationToken.None);

            // Assert
            // Ensure 'next' was never called since validation failed
            next.Verify(n => n(), Times.Never);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error?.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Error?.SubErrors?.Count.Should().Be(1);
            result.Error?.SubErrors?.Keys.First().Should().Be($"[{nameof(command.CartId)}]:");
        }


        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCartItemIdIsEmpty()
        {
            // Arrange
            var command = new RemoveCartItemCommand { CartItemId = Guid.Empty, CartId = Guid.NewGuid() };  // Invalid because ProductItemId is empty
            var validator = new RemoveCartItemCommandValidator();
            var behavior = new ValidationBehaviour<RemoveCartItemCommand, bool>([validator]);
            var next = new Mock<MyRequestHandlerDelegate<bool>>();

            // Act 
            var result = await behavior.Handle(command, next.Object, CancellationToken.None);

            // Assert
            // Ensure 'next' was never called since validation failed
            next.Verify(n => n(), Times.Never);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error?.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Error?.SubErrors?.Count.Should().Be(1);
            result.Error?.SubErrors?.Keys.First().Should().Be($"[{nameof(command.CartItemId)}]:");
        }
    }
}
