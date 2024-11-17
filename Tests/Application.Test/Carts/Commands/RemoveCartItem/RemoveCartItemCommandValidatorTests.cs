using CleanArchitecture.Application.Carts.Commands.AddItemToCart;
using CleanArchitecture.Application.Common.Behaviours;
using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Application.Common.Messaging;
using FluentValidation.Results;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Carts.Commands.RemoveCartItem;
using FluentAssertions;
using CleanArchitecture.Application.Common.Extensions;

namespace Application.Test.Carts.Commands.RemoveCartItem
{
    public class RemoveCartItemCommandValidatorTests
    {
        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCartIdIsEmpty()
        {
            // Arrange
            var command = new RemoveCartItemCommand { CartId = Guid.Empty };  // Invalid because ProductItemId is empty
            var validator = new Mock<IValidator<RemoveCartItemCommand>>();
            var failures = new List<ValidationFailure> { new(nameof(command.CartId), "Cart Id Couldn't be Empty") };

            validator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<RemoveCartItemCommand>>(), CancellationToken.None))
                .ReturnsAsync(() => new ValidationResult(failures));

            var behavior = new ValidationBehaviour<RemoveCartItemCommand, bool>([validator.Object]);
            var next = new Mock<MyRequestResponseHandlerDelegate<bool>>();

            // Act
            var result = await behavior.Handle(command, next.Object, CancellationToken.None);

            // Assert
            // Ensure 'next' was never called since validation failed
            next.Verify(n => n(), Times.Never);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error?.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Error?.Message.Should().Be(ValidationErrors.FluentValidationErrors(failures.ToDictionary()).Message);
            result.Error?.SubErrors?.Count.Should().Be(1);
            result.Error?.SubErrors?.Keys.First().Should().Be($"[{nameof(command.CartId)}]:");
        }


        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCartItemIdIsEmpty()
        {
            // Arrange
            var command = new RemoveCartItemCommand { CartItemId = Guid.Empty  };  // Invalid because ProductItemId is empty
            var validator = new Mock<IValidator<RemoveCartItemCommand>>();
            var failures = new List<ValidationFailure> { new(nameof(command.CartItemId), "Cart Item Id Couldn't be Empty") };

            validator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<RemoveCartItemCommand>>(), CancellationToken.None))
                .ReturnsAsync(() => new ValidationResult(failures));

            var behavior = new ValidationBehaviour<RemoveCartItemCommand, bool>([validator.Object]);
            var next = new Mock<MyRequestResponseHandlerDelegate<bool>>();

            // Act 
            var result = await behavior.Handle(command, next.Object, CancellationToken.None);

            // Assert
            // Ensure 'next' was never called since validation failed
            next.Verify(n => n(), Times.Never);
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error?.Code.Should().Be((int)HttpStatusCode.BadRequest);
            result.Error?.Message.Should().Be(ValidationErrors.FluentValidationErrors(failures.ToDictionary()).Message);
            result.Error?.SubErrors?.Count.Should().Be(1);
            result.Error?.SubErrors?.Keys.First().Should().Be($"[{nameof(command.CartItemId)}]:");
        }
    }
}
