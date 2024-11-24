using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistence.EF
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
