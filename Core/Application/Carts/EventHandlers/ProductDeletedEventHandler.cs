using CleanArchitecture.Application.Common.Abstracts.DomainEvent;
using CleanArchitecture.Domain.Product.Events;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Carts.EventHandlers
{
    public sealed class ProductDeletedEventHandler : BaseDomainEventHandler<ProductDeletedEvent>
    {
        #region Dependencies

        #endregion

        #region Constructor
        public ProductDeletedEventHandler(ILogger<ProductDeletedEventHandler> logger,
                                          IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {

        }
        #endregion

        #region Handle Event
        public override Task HandleEvent(ProductDeletedEvent notification, CancellationToken cancellationToken)
        {
            Logger.LogInformation("The Product with Id:{Id} has deleted", notification.Product.Id);

            return Task.CompletedTask;
        }
        #endregion
    }
}
