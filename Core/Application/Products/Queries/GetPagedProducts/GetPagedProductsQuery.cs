using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Products.Services;
using CleanArchitecture.Domain.Constants;
using Common.Linq.Model;

namespace CleanArchitecture.Application.Products.Queries.GetPagedProducts
{
    #region Request
    [Authorize(Roles = Roles.Administrator)]
    [Authorize(Policy = Permissions.Product.ReadPagedQuery)]
    public record GetPagedProductsQuery : PagedListQuery<IReadOnlyCollection<GetPagedProductDto>>
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public List<Guid>? CategoriesIds { get; init; }
        public List<DynamicOrderFields>? OrderProperties { get; init; }

    }
    #endregion

    #region Request Handler
    public class GetPagedProductsQueryHandler : BaseQueryHandler<GetPagedProductsQuery, IReadOnlyCollection<GetPagedProductDto>>
    {
        #region Dependencies

        private IProductService ProductService { get; }

        #endregion

        #region Constructor
        public GetPagedProductsQueryHandler(IServiceProvider serviceProvider, IApplicationDbContext dbContext, IProductService productService)
           : base(serviceProvider, dbContext)
        {
            ProductService = productService;
        }
        #endregion

        #region Handel
        public override async Task<IResult<IReadOnlyCollection<GetPagedProductDto>>> HandleRequest(GetPagedProductsQuery request,
                                                                                  CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<IReadOnlyCollection<GetPagedProductDto>>(Error.NullArgument);
            }
            (IReadOnlyCollection<GetPagedProductDto> Items, int totalCount) = await ProductService.GetPagedProductsWithFilter(request);

            return Items != null
               ? Result.Success(Items, totalCount)
               : Result.Failure<IReadOnlyCollection<GetPagedProductDto>>(Error.InternalServerError);

            #region Alt Result
            //return Result.SuccessIf(() => Items != null,
            //                  Items,
            //                  totalCount); 
            #endregion
        }


        #endregion
    }
    #endregion
}
