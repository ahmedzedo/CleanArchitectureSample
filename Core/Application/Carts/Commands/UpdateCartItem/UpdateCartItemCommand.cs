using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Caching;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Carts.Commands.UpdateCartItem
{
    #region Request
    [Authorize(Policy = Permissions.Cart.Update)]
    [InvalidCache(CacheStore = CacheStore.All, KeyPrefix = nameof(CacheKeysPrefixes.Cart))]
    public record UpdateCartItemCommand : BaseCommand<bool>, ICacheInvalidator
    {
        public Guid Id { get; init; }
        public Guid CartId { get; init; }
        public int Count { get; init; }
    }
    #endregion

    #region Request Handler
    public class UpdateCartItemCommandHandler : BaseCommandHandler<UpdateCartItemCommand, bool>
    {
        #region Dependencies


        #endregion

        #region Constructor
        public UpdateCartItemCommandHandler(
            IServiceProvider serviceProvider, IApplicationDbContext dbContext)
           : base(serviceProvider, dbContext)
        {

        }
        #endregion

        #region Request Handle

        public override async Task<IResult<bool>> HandleRequest(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await DbContext.Carts.Include(c => c.CartItems.Where(ci => ci.Id == request.Id))
                                            .FirstOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);

            if (cart == null || cart.Id != request.CartId || cart.CartItems.Count == 0)
            {
                return Result.Failure(CartsErrors.CartItemNotFoundError);
            }
            cart.CartItems.First().ChangeCount(request.Count);
            DbContext.Carts.Update(cart);
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

            return affectedRows > 0 ? Result.Success(affectedRows) : Result.Failure<bool>(Error.InternalServerError);

        }
        #endregion
    }
    #endregion
}
