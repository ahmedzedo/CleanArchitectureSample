using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Caching;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Carts.Commands.UpdateCartItem
{
    #region Request
    [Authorize(Policy = Permissions.Cart.Update)]
    [InvalidCache(CacheStore = CacheStore.All, KeyPrefix = nameof(CacheKeysPrefixes.Cart))]
    public record UpdateCartItemCommand : BaseCommand<bool>, ICacheInvalidator
    {
        public Guid CartItemId { get; init; }
        public Guid CartId { get; init; }
        public int Count { get; init; }
    }
    #endregion

    #region Request Handler
    public class UpdateCartItemCommandHandler : BaseCommandHandler<UpdateCartItemCommand, bool>
    {
        #region Dependencies

        private ICartService CartService { get; set; }
        #endregion

        #region Constructor
        public UpdateCartItemCommandHandler(
            IServiceProvider serviceProvider, IApplicationDbContext dbContext, ICartService cartService)
           : base(serviceProvider, dbContext)
        {
            CartService = cartService;
        }
        #endregion

        #region Request Handle

        public override async Task<IResult<bool>> HandleRequest(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            Cart? cart = await CartService.GetCartByCartItemIdAsync(request.CartItemId, cancellationToken);

            if (cart is null || cart.Id != request.CartId)
            {
                return Result.Failure(CartsErrors.CartNotFoundError);
            }

            if (cart.CartItems.Count == 0)
            {
                return Result.Failure(CartsErrors.CartEmptyError);
            }

            cart.CartItems.First().ChangeCount(request.Count);
            DbContext.Carts.Update(cart);
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

            return affectedRows > 0
                ? Result.Success(affectedRows)
                : Result.Failure(Error.InternalServerError);

        }


        #endregion
    }
    #endregion
}
