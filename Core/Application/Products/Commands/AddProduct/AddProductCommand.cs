using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Products.Events;
using CleanArchitecture.Domain.Products.Entites;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.Products.Commands.AddProduct
{
    #region Request
    [Authorize(Policy = Permissions.Product.Add)]
    public record AddProductCommand : BaseCommand<Guid>
    {
        public required string NameAr { get; init; }
        public required string NameEn { get; init; }
        public required string NameFr { get; init; }

        public List<Guid> CategoriesIds { get; init; } = [];

        public List<ProductItemDto> ItemsList { get; init; } = [];
    }
    #endregion

    #region Rquest Handler
    public class AddProductCommandHandler : BaseCommandHandler<AddProductCommand, Guid>
    {
        #region Dependencies

        #endregion

        #region Constructor
        public AddProductCommandHandler(IServiceProvider serviceProvider, IApplicationDbContext dbContext)
           : base(serviceProvider, dbContext)
        {

        }
        #endregion

        #region Request Handle
        public async override Task<Response<Guid>> HandleRequest(AddProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new(request.NameAr, request.NameEn, request.NameFr);
            List<Category> categories = await DbContext.Categories.AsTracking()
                                                        .Where(r => request.CategoriesIds.Contains(r.Id))
                                                        .ToListAsync(cancellationToken);
            product.AddCategory(categories);
            product.AddProductItems(request.ItemsList.Select(s => (ProductItem)s).ToList());
            product.AddDomainEvent(new ProductCreatedEvent(product));

            await DbContext.Products.AddAsync(product, cancellationToken);
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

            return affectedRows > 0 ? Response.Success(product.Id, affectedRows) : Response.Failure<Guid>(Error.InternalServerError);
        }


        #endregion
    }
    #endregion
}
