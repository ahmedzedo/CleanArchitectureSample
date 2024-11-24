using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Products.IEntitySets;
using CleanArchitecture.Domain.Products.Entites;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistence.EF.EntitySets
{
    public class ProductSet : EntitySet<Product>, IProductSet
    {
        #region Constructor
        public ProductSet(IDbContext dbContext) : base(dbContext)
        {
        }
        #endregion

        #region Custom Methods
        public async Task<ProductItem> GetProductItemAsync(Guid productItemId)
        {
            //return await Context.Set<Product>()
            //                             .Include(p => p.ProductItems)
            //                             .SelectMany(p => p.ProductItems)
            //                             .FirstOrDefaultAsync(pi => pi.Id == productItemId)

            return await Context.Set<ProductItem>().AsTracking().FirstOrDefaultAsync(pi => pi.Id == productItemId)
                          ?? throw new NotFoundException(nameof(productItemId), productItemId);
        }
        #endregion
    }
}
