using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Caching;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Application.Common.Errors;

namespace CleanArchitecture.Application.Carts.Commands.RemoveCartItem
{
    #region Request
    [Authorize(Policy = Permissions.Cart.Delete)]
    [Cache(KeyPrefix = nameof(CacheKeysPrefixes.Cart), CacheStore = CacheStore.All, ToInvalidate = true)]
    public record RemoveCartItemCommand : BaseCommand<bool>
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

        public override async Task<Response<bool>> HandleRequest(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            Cart? cart = await CartService.GetCartByIdIncludedItemById(request.CartId, request.CartItemId, cancellationToken);

            if (cart is null)
            {
                return Response.Failure(CartsErrors.CartNotFoundError);
            }

            if (cart.UserId != Guid.Parse(request.UserId!))
            {
                return Response.Failure(SecurityAccessErrors.ForbiddenAccess);
            }

            if (cart.CartItems.Count == 0 )
            {
                return Response.Failure(CartsErrors.CartEmptyError);
            }

            cart.RemoveCartItem(cart.CartItems.First());
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

            return affectedRows > 0 ? Response.Success(affectedRows) : Response.Failure(Error.InternalServerError);
        }


        #endregion
    }
    #endregion
}


