using System.Net;
using System.Reflection;
using AutoMapper;
using CleanArchitecture.Application.Carts;
using CleanArchitecture.Application.Carts.Commands.UpdateCartItem;
using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Products.Entites;
using Common.Reflection;
using FluentAssertions;
using Moq;

namespace Application.Test.Carts.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandTest
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly UpdateCartItemCommandHandler _handler;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IMapper> _mapper;
        public UpdateCartItemCommandTest()
        {

            _cartServiceMock = new Mock<ICartService>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _mapper = new Mock<IMapper>();
            _serviceProviderMock
            .Setup(sp => sp.GetService(typeof(IMapper)))
                           .Returns(_mapper.Object);
            _handler = new UpdateCartItemCommandHandler(_serviceProviderMock.Object, It.IsAny<IApplicationDbContext>(), _cartServiceMock.Object);
        }

        [Fact]
        public async Task HandleRequest_IfCartIsNull_ReturnCartNotFoundError()
        {
            // Arrang
            var command = new UpdateCartItemCommand()
            {
                CartItemId = Guid.NewGuid(),
                CartId = Guid.NewGuid()
            };
            _cartServiceMock.Setup(service => service.GetCartByCartItemIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
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
        public async Task HandleRequest_IfCartIdNotEqualRequestCartId_ReturnCartNotFoundError()
        {
            // Arrang
            var command = new UpdateCartItemCommand()
            {
                CartItemId = Guid.NewGuid(),
                CartId = Guid.NewGuid()
            };
            var cart = new Cart(Guid.NewGuid().ToString());

            List<CartItem> cartItems =
                [CartItem.CreateCartItem(cart,new ProductItem("Prodcut Item 0 Description ", 150,258),20),
                CartItem.CreateCartItem(cart,new ProductItem("Prodcut Item 0 Description ", 150,258),13),
                CartItem.CreateCartItem(cart,new ProductItem("Prodcut Item 0 Description ", 150,258),25)];

            cart.AddCartItems(cartItems);

            _cartServiceMock.Setup(service => service.GetCartByCartItemIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            // Act
            var result = await _handler.HandleRequest(command, CancellationToken.None);
            // Assert

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error!.Code.Should().Be((int)HttpStatusCode.NotFound);
            result.Error.Message.Should().Be(CartsErrors.CartNotFoundError.Message);
        }

        [Fact]
        public async Task HandleRequest_IfCartItemEmpty_ReturnCartNotFoundError()
        {
            // Arrang
            var command = new UpdateCartItemCommand()
            {
                CartItemId = Guid.NewGuid(),
                CartId = Guid.NewGuid()
            };
            var cart = new Cart(Guid.NewGuid().ToString());

            cart.SetPropertyValue("Id", command.CartId, BindingFlags.Instance | BindingFlags.NonPublic);


            _cartServiceMock.Setup(service => service.GetCartByCartItemIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
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
