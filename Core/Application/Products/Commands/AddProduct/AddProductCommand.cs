using CleanArchitecture.Application.Categories.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Application.Products.Services;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Products.Entites;

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
        private ICategoryService CategoryService { get; }
        private IProductService ProductService { get; }
        #endregion

        #region Constructor
        public AddProductCommandHandler(IServiceProvider serviceProvider,
                                        IApplicationDbContext dbContext,
                                        ICategoryService categoryService,
                                        IProductService productService)
           : base(serviceProvider, dbContext)
        {
            CategoryService = categoryService;
            ProductService = productService;
        }
        #endregion

        #region Request Handle
        public async override Task<IResult<Guid>> HandleRequest(AddProductCommand request, CancellationToken cancellationToken)
        {
            var categories = await CategoryService.GetCategoriesByIds(request.CategoriesIds, cancellationToken);
            var productItems = request.ItemsList.Select(s => (ProductItem)s).ToList();
            Product product = new(request.NameAr, request.NameEn, request.NameFr);

            await ProductService.AddProduct(product, categories, productItems, cancellationToken);
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

            return affectedRows > 0
                ? Result.Success(product.Id, affectedRows)
                : Result.Failure<Guid>(Error.InternalServerError);
        }


        #endregion
    }
    #endregion
}
