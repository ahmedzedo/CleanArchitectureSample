using System.Net;
using CleanArchitecture.Application.Carts.Commands.UpdateCartItem;
using CleanArchitecture.Application.Common.Behaviours;
using CleanArchitecture.Application.Common.Messaging;
using FluentAssertions;
using Moq;

namespace Application.Test.Carts.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandValidatorTest
    {
        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCartItemIdIsEmpty()
        {
            // Arrange
            var command = new UpdateCartItemCommand { CartId = Guid.NewGuid(), CartItemId = Guid.Empty, Count = 10 };  // Invalid because ProductItemId is empty
            var validator = new UpdateCartItemCommandValidator();
            var behavior = new ValidationBehaviour<UpdateCartItemCommand, bool>([validator]);
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

        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCountLessThanOrEqualZero()
        {
            // Arrange
            var command = new UpdateCartItemCommand { CartId = Guid.NewGuid(), CartItemId = Guid.NewGuid(), Count = 0 };  // Invalid because ProductItemId is empty
            var validator = new UpdateCartItemCommandValidator();
            var behavior = new ValidationBehaviour<UpdateCartItemCommand, bool>([validator]);
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
            result.Error?.SubErrors?.Keys.First().Should().Be($"[{nameof(command.Count)}]:");
        }
    }
}
