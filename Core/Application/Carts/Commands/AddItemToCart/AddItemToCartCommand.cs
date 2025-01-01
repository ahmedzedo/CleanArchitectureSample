using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Caching;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Carts.Entities;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Carts.Commands.AddItemToCart
{
    #region Request
    [Authorize(Policy = Permissions.Cart.Add)]
    [InvalidCache(KeyPrefix = nameof(CacheKeysPrefixes.Cart), CacheStore = CacheStore.All)]
    public record AddItemToCartCommand : BaseCommand<Guid>, ICacheInvalidator
    {
        public Guid ProductItemId { get; init; }
        public Guid? CartId { get; init; }
        public int Count { get; init; }
    }
    #endregion

    #region Rquest Handler
    public sealed class AddItemToCartCommandHandler : BaseCommandHandler<AddItemToCartCommand, Guid>
    {
        #region Dependencies

        ICartService CartService { get; }

        #endregion

        #region Constructor
        public AddItemToCartCommandHandler(IServiceProvider serviceProvider,
                                           IApplicationDbContext dbContext,
                                           ICartService cartService)
           : base(serviceProvider, dbContext)
        {
            CartService = cartService;
        }
        #endregion

        #region Request Handle
        public async override Task<IResult<Guid>> HandleRequest(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            Guid userId = Guid.Parse(request.UserId!);
            var cart = await CartService.AddOrUpdateUserCartAsync(userId, request.ProductItemId, request.Count, cancellationToken);
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

            return affectedRows > 0
                ? Result.Success(cart.Id, affectedRows)
                : Result.Failure<Guid>(Error.InternalServerError);
        }

        #endregion
    }
    #endregion
}
