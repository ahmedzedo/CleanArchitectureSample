using AutoMapper;
using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Domain.Carts.Entities;
using FluentAssertions;
using Moq;
using System.Net;

namespace CleanArchitecture.Application.Carts.Commands.RemoveCartItem.Tests
{
    public class RemoveCartItemCommandHandlerTests
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly RemoveCartItemCommandHandler _handler;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IMapper> _mapper;
        public RemoveCartItemCommandHandlerTests()
        {
            _cartServiceMock = new Mock<ICartService>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _mapper = new Mock<IMapper>();
            _serviceProviderMock
                           .Setup(sp => sp.GetService(typeof(IMapper)))
                           .Returns(_mapper.Object);
            _handler = new RemoveCartItemCommandHandler(_serviceProviderMock.Object, It.IsAny<IApplicationDbContext>(), _cartServiceMock.Object);
        }
        [Fact]
        public async Task HandleRequest_IfCartIsNull_ReturnCartNotFoundError()
        {
            // Arrang
            var command = new RemoveCartItemCommand()
            {
                CartItemId = Guid.NewGuid(),
                CartId = Guid.NewGuid()
            };
            _cartServiceMock.Setup(service => service.GetCartByIdIncludedItemById(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Cart?)null);
            // Act
            var result = await _handler.HandleRequest(command, CancellationToken.None);
            // Assert

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be((int)HttpStatusCode.NotFound);
            result.Error.Message.Should().Be(CartsErrors.CartNotFoundError.Message);
        }

        [Fact]
        public async Task HandleRequest_IfCurrentUserNotMatchedWithCartUser_ReturnForbiddenAccessError()
        {
            // Arrang
            var command = new RemoveCartItemCommand()
            {
                CartItemId = Guid.NewGuid(),
                CartId = Guid.NewGuid(),
                UserId = Guid.NewGuid().ToString()
            };

            var cart = new Cart(Guid.NewGuid());

            _cartServiceMock.Setup(service => service.GetCartByIdIncludedItemById(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            // Act
            var result = await _handler.HandleRequest(command, CancellationToken.None);
            // Assert

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be((int)HttpStatusCode.Forbidden);
            result.Error.Message.Should().Be(SecurityAccessErrors.ForbiddenAccess.Message);
        }

        [Fact]
        public async Task HandleRequest_IfCartItemsEmpty_ReturnCartEmptyError()
        {
            // Arrang
            var userId = Guid.NewGuid();
            var command = new RemoveCartItemCommand()
            {
                CartItemId = Guid.NewGuid(),
                CartId = Guid.NewGuid(),
                UserId = userId.ToString()
            };

            var cart = new Cart(userId);

            _cartServiceMock.Setup(service => service.GetCartByIdIncludedItemById(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            // Act
            var result = await _handler.HandleRequest(command, CancellationToken.None);
            // Assert

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be((int)HttpStatusCode.NotFound);
            result.Error.Message.Should().Be(CartsErrors.CartEmptyError.Message);
        }
    }
}