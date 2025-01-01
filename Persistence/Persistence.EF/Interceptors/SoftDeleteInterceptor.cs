using CleanArchitecture.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Persistence.EF.Interceptors
{
    public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        #region Save Changes
        public override InterceptionResult<int> SavingChanges(
         DbContextEventData eventData,
         InterceptionResult<int> result
         )
        {
            ApplySoftDelete(eventData);

            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
          DbContextEventData eventData,
          InterceptionResult<int> result,
          CancellationToken cancellationToken = default)
        {
            ApplySoftDelete(eventData);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        #endregion

        #region Applying Soft Delete
        private static void ApplySoftDelete(DbContextEventData eventData)
        {
            if (eventData.Context == null) return;

            IEnumerable<EntityEntry<ISoftDeletable>> entries =
                eventData
                    .Context
                    .ChangeTracker
                    .Entries<ISoftDeletable>()
                    .Where(e => e.State == EntityState.Deleted);

            if (!entries.Any()) return;

            foreach (EntityEntry<ISoftDeletable> softDeletable in entries)
            {
                softDeletable.State = EntityState.Modified;
                softDeletable.Entity.IsDeleted = true;
                softDeletable.Entity.DeletedOnUtc = DateTime.UtcNow;
            }
        } 
        #endregion
    }
}
