using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Products.Entites;

namespace CleanArchitecture.Domain.Products.Events
{
    public record ProductItemDeletedEvent(ProductItem ProductItem) : BaseDomainEvent
    {
    }
    public record ProductItemsDeletedEvent(IEnumerable<ProductItem> ProductItems) : BaseDomainEvent
    {
    }
}
