using AutoMapper;
using CleanArchitecture.Application.Carts.Commands.AddItemToCart;
using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Carts.Entities;
using Moq;
using System.Reflection;
namespace Application.Test.Carts.Commands.AddItemToCart
{
    public class AddItemToCartCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _DbContextMock;
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly AddItemToCartCommandHandler _handler;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IMapper> _mapper;

        public AddItemToCartCommandHandlerTests()
        {
            _DbContextMock = new Mock<IApplicationDbContext>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _cartServiceMock = new Mock<ICartService>();
            _mapper = new Mock<IMapper>();
            _serviceProviderMock
                            .Setup(sp => sp.GetService(typeof(IMapper)))
                            .Returns(_mapper.Object);
            _handler = new AddItemToCartCommandHandler(_serviceProviderMock.Object, _DbContextMock.Object, _cartServiceMock.Object);
        }

        // [Fact]
        //public async Task HandleRequest_ShouldReturnSuccess_WhenCartExistsAndItemAdded()
        //{
        //    // Arrange
        //    var command = new AddItemToCartCommand
        //    {
        //        UserId = Guid.NewGuid().ToString(),
        //        ProductItemId = Guid.NewGuid(),
        //        Count = 1
        //    };
        //    var cartId = Guid.NewGuid();
        //    var cart = new Cart(Guid.NewGuid());

        //    //cart.SetPropertyValue("Id", cartId, BindingFlags.Instance | BindingFlags.NonPublic);

        //    //_cartServiceMock.Setup(service => service.GetUserCartAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
        //    //                .ReturnsAsync(cart);
        //    //_cartServiceMock.Setup(service => service.AddOrUpdateCartItemAsync(cart,
        //    //                                                                   command.ProductItemId,
        //    //                                                                   command.Count,
        //    //                                                                   It.IsAny<CancellationToken>()));

        //    _cartServiceMock.Setup(service => service.AddOrUpdateUserCartAsync(Guid.Parse(command.UserId), command.ProductItemId, command.Count, It.IsAny<CancellationToken>()))
        //       .ReturnsAsync(cart);

        //    _DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
        //                  .ReturnsAsync(1);
        //    // Act
        //    var result = await _handler.HandleRequest(command, CancellationToken.None);

        //    // Assert
        //    result.IsSuccess.Should().BeTrue();
        //    result.Data.Should().Be(cartId);
        //}



        [Fact]
        public async Task HandleRequest_ShouldCreateNewCart_WhenCartDoesNotExist()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var command = new AddItemToCartCommand { UserId = Guid.NewGuid().ToString(), ProductItemId = Guid.NewGuid(), Count = 1 };
            var newCart = new Cart(Guid.NewGuid());
            var idField = typeof(Cart).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            idField?.SetValue(newCart, cartId);

            //_cartServiceMock.Setup(service => service.GetUserCartAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            //                .ReturnsAsync((default(Cart)));
            //_cartServiceMock.Setup(service => service.AddUserCartAsync(It.IsAny<Guid>()))
            //                .ReturnsAsync(newCart);
            //_cartServiceMock.Setup(service => service.AddOrUpdateCartItemAsync(newCart, command.ProductItemId, command.Count, It.IsAny<CancellationToken>()));
            _cartServiceMock.Setup(service => service.AddOrUpdateUserCartAsync(Guid.Parse(command.UserId), command.ProductItemId, command.Count, It.IsAny<CancellationToken>()))
                .ReturnsAsync(newCart);

            _DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(1);

            // Act
            var result = await _handler.HandleRequest(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newCart.Id, result.Data);
        }

        [Fact]
        public async Task HandleRequest_ShouldReturnFailure_WhenSaveChangesFails()
        {
            // Arrange
            var command = new AddItemToCartCommand { UserId = Guid.NewGuid().ToString(), ProductItemId = Guid.NewGuid(), Count = 1 };
            var cart = new Cart(Guid.NewGuid());

            _cartServiceMock.Setup(service => service.GetUserCartAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(cart);
            _cartServiceMock.Setup(service => service.AddOrUpdateCartItemAsync(cart, command.ProductItemId, command.Count, It.IsAny<CancellationToken>()));

            _DbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(0);

            // Act
            var result = await _handler.HandleRequest(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Error.InternalServerError, result.Error);
        }
    }
}
