using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Caching;
using CleanArchitecture.Application.Common.Errors;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Carts.Commands.RemoveCartItem
{
    #region Request
    [Authorize(Policy = Permissions.Cart.Delete)]
    [InvalidCache(KeyPrefix = nameof(CacheKeysPrefixes.Cart), CacheStore = CacheStore.All)]
    public record RemoveCartItemCommand : BaseCommand<bool>, ICacheInvalidator
    {
        public Guid CartItemId { get; init; }
        public Guid CartId { get; init; }

    }
    #endregion

    #region Request Handler
    public sealed class RemoveCartItemCommandHandler : BaseCommandHandler<RemoveCartItemCommand, bool>
    {
        #region Dependencies

        private ICartService CartService { get; }

        #endregion

        #region Constructor
        public RemoveCartItemCommandHandler(IServiceProvider serviceProvider,
                                            IApplicationDbContext dbContext,
                                            ICartService cartService)
           : base(serviceProvider, dbContext)
        {
            CartService = cartService;
        }
        #endregion

        #region Request Handle

        public override async Task<IResult<bool>> HandleRequest(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            Cart? cart = await CartService.GetCartByIdIncludedItemById(request.CartId, request.CartItemId, cancellationToken);

            if (cart is null)
            {
                return Result.Failure(CartsErrors.CartNotFoundError);
            }

            if (cart.UserId != request.UserId!)
            {
                return Result.Failure(SecurityAccessErrors.ForbiddenAccess);
            }

            if (cart.CartItems.Count == 0)
            {
                return Result.Failure(CartsErrors.CartEmptyError);
            }

            cart.RemoveCartItem(cart.CartItems.First());
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

            return affectedRows > 0
                ? Result.Success(affectedRows)
                : Result.Failure(Error.InternalServerError);
        }


        #endregion
    }
    #endregion
}


