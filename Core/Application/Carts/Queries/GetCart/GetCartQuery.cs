using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Carts;
using CleanArchitecture.Application.Carts.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Caching;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Carts.Queries.GetCart
{
    #region Request
    [Authorize(Policy = Permissions.Cart.Read)]
    [Cache(CacheStore = CacheStore.All, SlidingExpirationMinutes = "1", /*AbsoluteExpirationMinutes = "60", LimitSize = "10",*/ ToInvalidate = false, KeyPrefix = nameof(CacheKeysPrefixes.Cart))]
    public record GetCartQuery(Guid Id) : BaseQuery<CartDto>, ICacheable
    {
        public string CahcheKeyIdentifire => $"{Id}";
    }
    #endregion

    #region Request Handler
    public sealed class GetCartQueryHandler : BaseQueryHandler<GetCartQuery, CartDto>
    {
        #region Dependencies

        private ICartService CartService { get; }

        #endregion

        #region Constructor
        public GetCartQueryHandler(IServiceProvider serviceProvider, IApplicationDbContext dbContext, ICartService cartService)
           : base(serviceProvider, dbContext)
        {
            CartService = cartService;
        }
        #endregion

        #region Handel

        public async override Task<Response<CartDto>> HandleRequest(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await CartService.GetUserCartAsync(Guid.Parse(request.UserId!), cancellationToken);
            var result = Mapper.Map<CartDto>(cart);

            return result != null ? Response.Success(result, 1) : Response.Failure<CartDto>(CartsErrors.CartEmptyError);
        }
        #endregion
    }
    #endregion
}
