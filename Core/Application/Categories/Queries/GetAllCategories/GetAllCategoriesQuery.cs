using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Products.Entites;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Caching;

namespace CleanArchitecture.Application.Categories.Queries.GetAllCategories
{
    #region Request

    [Authorize(Policy = Permissions.Product.ReadCategories)]
    [Cache(CacheStore = CacheStore.All, SlidingExpirationMinutes = "1", /*AbsoluteExpirationMinutes = "60", LimitSize = "10",*/  KeyPrefix = nameof(CacheKeysPrefixes.Category))]
    public record GetAllCategoriesQuery : BaseQuery<IReadOnlyCollection<Category>>, ICacheable
    {

    }
    #endregion

    #region Request Handler
    public class GetAllCategoriesQueryHandler : BaseQueryHandler<GetAllCategoriesQuery, IReadOnlyCollection<Category>>
    {
        #region Constructor
        public GetAllCategoriesQueryHandler(IServiceProvider serviceProvider, IApplicationDbContext dbContext)
           : base(serviceProvider, dbContext)
        {

        }
        #endregion

        #region Handel
        public override async Task<IResult<IReadOnlyCollection<Category>>> HandleRequest(GetAllCategoriesQuery request,
                                                                                  CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<IReadOnlyCollection<Category>>(Error.NullArgument);
            }
            var Items = await DbContext.Categories.ToListAsync(cancellationToken);

            return Result.Success<IReadOnlyCollection<Category>>(Items.AsReadOnly(), Items.Count);
        }
        #endregion
    }
    #endregion
}
