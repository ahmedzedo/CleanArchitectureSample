using AutoMapper;
using CleanArchitecture.Application.Categories.Services;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Caching;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Common.Security;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Application.Categories.Commands.AddCategory
{
    #region Request
    [Authorize(Policy = Permissions.Product.AddCategory)]
    [InvalidCache(KeyPrefix = nameof(CacheKeysPrefixes.Category), CacheStore = CacheStore.All)]
    public record AddCategoryCommand : BaseCommand<Guid>, ICacheInvalidator
    {
        public required string NameAr { get; init; }
        public required string NameEn { get; init; }
        public required string NameFr { get; init; }
        public required string BriefAr { get; init; }
        public required string BriefEn { get; init; }
        public required string BriefFr { get; init; }
        public DateTime ApplyingDate { get; init; }

        #region Mapping
        public sealed class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<AddCategoryCommand, Category>();
            }
        }
        #endregion
    }

    #endregion

    #region Request Handler
    public class AddCategoryCommandHandler : BaseCommandHandler<AddCategoryCommand, Guid>
    {
        #region Dependencies 
        private ICategoryService CategoryService { get; }
        #endregion

        #region Constructor
        public AddCategoryCommandHandler(IServiceProvider serviceProvider,
                                         IApplicationDbContext dbContext,
                                         ICategoryService categoryService)
            : base(serviceProvider, dbContext)
        {
            CategoryService = categoryService;
        }

        #endregion

        #region Handel Request
        public async override Task<IResult<Guid>> HandleRequest(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = Mapper.Map<Category>(request);
            await CategoryService.AddCategory(category, cancellationToken);
            int affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

            return affectedRows > 0
                ? Result.Success(category.Id, affectedRows)
                : Result.Failure<Guid>(Error.InternalServerError);
        }
        #endregion
    }
    #endregion
}
