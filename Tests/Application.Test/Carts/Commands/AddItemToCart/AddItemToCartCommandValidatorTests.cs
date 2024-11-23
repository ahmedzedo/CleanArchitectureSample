using CleanArchitecture.Application.Carts.Commands.AddItemToCart;
using CleanArchitecture.Application.Common.Behaviours;
using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Application.Common.Extensions;
using CleanArchitecture.Application.Common.Messaging;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Net;

namespace Application.Test.Carts.Commands.AddItemToCart
{
    public class AddItemToCartCommandValidatorTests
    {
        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenProductItemIsEmpty()
        {
            // Arrange
            var command = new AddItemToCartCommand { ProductItemId = Guid.Empty };  // Invalid because ProductItemId is empty
            var validator = new Mock<IValidator<AddItemToCartCommand>>();
            var failures = new List<ValidationFailure> { new(nameof(command.ProductItemId), "product Item Id Couldn't be Empty") };

            validator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<AddItemToCartCommand>>(), CancellationToken.None))
                .ReturnsAsync(() => new ValidationResult(failures));

            var behavior = new ValidationBehaviour<AddItemToCartCommand, Guid>(new[] { validator.Object });
            var next = new Mock<MyRequestResponseHandlerDelegate<Guid>>();

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
            result.Error?.SubErrors?.Keys.First().Should().Be($"[{nameof(command.ProductItemId)}]:");
        }


        [Fact]
        public async Task Handle_ShouldReturnValidationError_WhenCountIsLessThanOrEqualZero()
        {
            // Arrange
            var command = new AddItemToCartCommand { Count = -4 };  // Invalid because ProductItemId is empty
            var validator = new Mock<IValidator<AddItemToCartCommand>>();
            var failures = new List<ValidationFailure> { new(nameof(command.Count), "Count Couldn't be Less than or Equal Zero") };

            validator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<AddItemToCartCommand>>(), CancellationToken.None))
                .ReturnsAsync(() => new ValidationResult(failures));

            var behavior = new ValidationBehaviour<AddItemToCartCommand, Guid>(new[] { validator.Object });
            var next = new Mock<MyRequestResponseHandlerDelegate<Guid>>();

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

