using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Application.Categories.Services
{
    public interface ICategoryService : IBaseService
    {
        Task<Category> AddCategory(Category category, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Category>> GetAllCategories(CancellationToken cancellationToken = default);
        Task<List<Category>> GetCategoriesByIds(List<Guid> ids, CancellationToken cancellationToken = default);


    }
}
