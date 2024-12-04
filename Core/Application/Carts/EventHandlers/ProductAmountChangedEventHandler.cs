using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Application.Common.Abstracts.DomainEvent;
using CleanArchitecture.Domain.Products.Events;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Carts.EventHandlers
{
    public sealed class ProductAmountChangedEventHandler : BaseDomainEventHandler<ProductItemAmountChangedEvent>
    {
        #region Dependencies
        private ICartService CartService { get; }
        #endregion

        #region Constructor
        public ProductAmountChangedEventHandler(ILogger<ProductAmountChangedEventHandler> logger,
                                                IServiceProvider serviceProvider,
                                                ICartService cartService) : base(logger, serviceProvider)
        {
            CartService = cartService;
        }

        #endregion

        #region Event Handler

        public override async Task HandleEvent(ProductItemAmountChangedEvent notification,
                                               CancellationToken cancellationToken)
        {
            var cartItems = await CartService.GetCartItemsOfProductItem(notification.ProductItem.Id, cancellationToken);
            var isRequiredItemCountExceededAmount = cartItems.Sum(ci => ci.Count) > notification.ProductItem.Amount;

            if (isRequiredItemCountExceededAmount)
            {
                await CartService.DeleteCartItems(cartItems, cancellationToken);
            }
        }

        #endregion
    }
}
