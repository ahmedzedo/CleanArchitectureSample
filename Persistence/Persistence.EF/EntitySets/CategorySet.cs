using CleanArchitecture.Application.Categories.IEntitySets;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Persistence.EF.EntitySets
{
    public class CategorySet : EntitySet<Category>, ICategorySet
    {
        #region Constructor
        public CategorySet(ApplicationDbContext DbContext) : base(DbContext)
        {

        }
        #endregion

    }
}
