using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Products.Events;

namespace CleanArchitecture.Application.Products.Commands.DeleteProduct
{
    #region Request
    [Authorize(Policy = Permissions.Product.Delete)]

    public record DeleteProductCommand : BaseCommand<bool>
    {
        public Guid Id { get; init; }
    }
    #endregion

    #region Request Handler
    public class DeleteProductCommandHandler : BaseCommandHandler<DeleteProductCommand, bool>
    {

        #region Dependencies

        #endregion

        #region Constructors
        public DeleteProductCommandHandler(IServiceProvider serviceProvider, IApplicationDbContext dbContext)
            : base(serviceProvider, dbContext)
        {

        }


        #endregion

        #region Request Handle
        public async override Task<IResult<bool>> HandleRequest(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            int affectedRows = 0;

            var product = await DbContext.Products.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
            {
                return Result.Failure(Error.ItemNotFound($"ProductId:{request.Id}"));
            }
            if (DbContext.Products.Delete(product))
            {
                product.AddDomainEvent(new ProductDeletedEvent(product));
                affectedRows = await DbContext.SaveChangesAsync(cancellationToken);
            }

            return affectedRows > 0
                ? Result.Success(affectedRows)
                : Result.Failure(Error.InternalServerError);
        }
        #endregion
    }
    #endregion

}
