using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Common.ORM.EntityFramework
{
    public static class ModelBuilderExtension
    {
        public static void ApplyGlobalFilter<T>(this ModelBuilder modelBuilder, string propertyName, T value)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                    .Where(x => x.FindProperty(propertyName) != null)
                    .Select(x => x.ClrType);

            foreach (var entityType in entityTypes)
            {
                var newParam = Expression.Parameter(entityType);
                var filter = Expression.Lambda(Expression.Equal(Expression.Convert(Expression.Property(newParam, propertyName),
                                                                                   typeof(T)), Expression.Constant(value)), newParam);
                modelBuilder.Entity(entityType).HasQueryFilter(filter);
            }
        }

        public static void ApplyGlobalFilter<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> expressionFilter)
        {
            var entityTypes = modelBuilder.Model
                        .GetEntityTypes()
                        .Select(e => e.ClrType)
                        .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface);

            foreach (var entityType in entityTypes)
            {
                var newParam = Expression.Parameter(entityType);
                var body = ReplacingExpressionVisitor.Replace(expressionFilter.Parameters[0], newParam, expressionFilter.Body);
                var lambdaExpression = Expression.Lambda(body, newParam);

                modelBuilder.Entity(entityType).HasQueryFilter(lambdaExpression);
            }
        }
    }
}
