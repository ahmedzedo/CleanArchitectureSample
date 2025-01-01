using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Domain.Products.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Categories.Services
{
    public interface ICategoryService: IBaseService
    {
        Task<Category> AddCategory(Category category, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Category>> GetAllCategories(CancellationToken cancellationToken = default);
        Task<List<Category>> GetCategoriesByIds(List<Guid> ids,CancellationToken cancellationToken = default);


    }
}
