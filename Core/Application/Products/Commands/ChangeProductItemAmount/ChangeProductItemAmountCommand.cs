using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Products.Commands.ChangeProductItemAmount
{
    #region Request
    [Authorize(Policy = Permissions.Product.ChangeProductItemAmount)]
    public record ChangeProductItemAmountCommand : BaseCommand<bool>
    {
        public Guid ProductId { get; init; }
        public Guid ProductItmeId { get; init; }
        public int Amount { get; init; }
    }
    #endregion

    #region Request Handler
    public class ChangeProductItemAmountCommandHandler : BaseCommandHandler<ChangeProductItemAmountCommand, bool>
    {
        #region Dependencies


        #endregion

        #region Constructor
        public ChangeProductItemAmountCommandHandler(
            IServiceProvider serviceProvider, IApplicationDbContext dbContext)
           : base(serviceProvider, dbContext)
        {

        }

        #endregion

        #region RequestHandle
        public override async Task<IResult<bool>> HandleRequest(ChangeProductItemAmountCommand request, CancellationToken cancellationToken)
        {
            var product = await DbContext.Products.AsTracking()
                                                  .Include(p => p.ProductItems.Where(pi => pi.Id == request.ProductItmeId))
                                                  .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product is null || product.ProductItems.Count == 0)
            {
                return Result.Failure(Error.ItemNotFound($"ProductId:{request.ProductId} contains the ProductItemid:{request.ProductItmeId}"));
            }

            product.ProductItems.First().ChangeAmount(request.Amount);
            DbContext.Products.Update(product);
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

            return affectedRows > 0 ? Result.Success(affectedRows) : Result.Failure(Error.InternalServerError);

        }
        #endregion
    }
    #endregion
}
