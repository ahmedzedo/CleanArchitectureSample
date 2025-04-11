using CleanArchitecture.Application.Carts.IEntitySets;
using CleanArchitecture.Application.Categories.IEntitySets;
using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Products.IEntitySets;
using CleanArchitecture.Application.Users.IEntitySet;
using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Persistence.EF.Configurations;
using Common.DependencyInjection.Extensions;
using Common.ORM.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace CleanArchitecture.Persistence.EF
{

    public sealed class ApplicationDbContext : IdentityUserContext<ApplicationUser>, IApplicationDbContext, IDbContext
    {
        #region Properties
        public IServiceProvider ServiceProvider { get; }

        #endregion

        #region Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                         IServiceProvider serviceProvider) : base(options)
        {
            ServiceProvider = serviceProvider;
        }
        #endregion

        DbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        #region Entities Sets 

        IProductSet IApplicationDbContext.Products => ServiceProvider.GetInstance<IProductSet>();
        ICategorySet IApplicationDbContext.Categories => ServiceProvider.GetInstance<ICategorySet>();
        ICartSet IApplicationDbContext.Carts => ServiceProvider.GetInstance<ICartSet>();
        IRefreshTokenSet IApplicationDbContext.RefreshTokens => ServiceProvider.GetInstance<IRefreshTokenSet>();

        #endregion

        #region On Model Creating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<User>();
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ApplyGlobalFilter<ISoftDeletable>(e => !e.IsDeleted);
            base.OnModelCreating(builder);
            builder.ConfigureIdentity();
        }
        #endregion

        #region Save Changes
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}
