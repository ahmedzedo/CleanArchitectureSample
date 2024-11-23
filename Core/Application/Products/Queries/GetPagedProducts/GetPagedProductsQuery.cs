using CleanArchitecture.Application.Common.Security;
using Common.Linq.Model;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;

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

        #endregion

        #region Constructor
        public GetPagedProductsQueryHandler(IServiceProvider serviceProvider, IApplicationDbContext dbContext)
           : base(serviceProvider, dbContext)
        {

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
            (IReadOnlyCollection<GetPagedProductDto> Items, int totalCount) = await GetPagedProductsWithFilter(request);

            return Result.Success(Items, totalCount);
        }

        private async Task<(IReadOnlyCollection<GetPagedProductDto> Items, int totalCount)> GetPagedProductsWithFilter(GetPagedProductsQuery request)
        {
            return await DbContext.Products
                   .Include(p => p.Include(s => s.ProductItems)
                                  .Include(p => p.Categories))
                   .WhereIf(!string.IsNullOrEmpty(request.Name), p => p.NameAr.Contains(request.Name!)
                                                                   || p.NameEn.Contains(request.Name!)
                                                                   || p.NameFr.Contains(request.Name!))
                   .WhereIf(!string.IsNullOrEmpty(request.Description), p => p.ProductItems
                                                                              .Any(pd => pd.Description!.Contains(request.Description!)))
                   .WhereIf(request.CategoriesIds != null && request.CategoriesIds.Count != 0, p => p.Categories.Select(r => r.Id)
                                                                                                .Any(r => request.CategoriesIds!.Contains(r)))
                   // .WhereIf(request.Price != default && request.Price > 0, p => p.ProductDetails.Any(pd => pd.Price == request.Price))

                   //.DynamicOrderBy(request.OrderByPropertyName ?? "CreatedOn", request.SortDirection!.ToLower() == "asc" ? SortDirection.Ascending : SortDirection.Descending)
                   .DynamicOrderBy(request.OrderProperties)
                   .ToPagedListAsync<GetPagedProductDto>(request.PageIndex, request.PageSize, p => p);
        }
        #endregion
    }
    #endregion
}
