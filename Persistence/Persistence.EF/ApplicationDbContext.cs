using CleanArchitecture.Application.Carts.IEntitySets;
using CleanArchitecture.Application.Categories.IEntitySets;
using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Products.IEntitySets;
using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Infrastructure.Identity;
using CleanArchitecture.Persistence.EF.Configurations;
using Common.DependencyInjection.Extensions;
using Common.ORM.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CleanArchitecture.Persistence.EF
{

    public sealed class ApplicationDbContext : IdentityUserContext<ApplicationUser>, IApplicationDbContext, IDbContext
    {
        #region Properties
        private ICurrentUser CurrentUserService { get; }
        public IServiceProvider ServiceProvider { get; }

        #endregion

        #region Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                         ICurrentUser currentUserService,
                         IServiceProvider serviceProvider) : base(options)
        {
            CurrentUserService = currentUserService;
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

        #endregion

        #region On Model Creating
        protected override void OnModelCreating(ModelBuilder builder)
        {
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
