using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Products.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Categories.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        #region Dependencies
        private IApplicationDbContext DbContext { get; }
        #endregion
        #region Constructor
        public CategoryService(IServiceProvider serviceProvider,
                               IApplicationDbContext dbContext) : base(serviceProvider)
        {
            DbContext = dbContext;
        }
        #endregion

        #region Methods
        public async Task<Category> AddCategory(Category category, CancellationToken cancellationToken = default)
        {
            return await DbContext.Categories.AddAsync(category, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Category>> GetAllCategories(CancellationToken cancellationToken = default)
        {
            return await DbContext.Categories.ToListAsync(cancellationToken);
        }

        public async Task<List<Category>> GetCategoriesByIds(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await DbContext.Categories.AsTracking()
                                       .Where(r => ids.Contains(r.Id))
                                       .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
