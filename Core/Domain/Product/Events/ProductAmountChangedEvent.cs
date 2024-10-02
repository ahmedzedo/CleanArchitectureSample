using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Product.Entites;

namespace CleanArchitecture.Domain.Product.Events
{
    public record ProductItemAmountChangedEvent(ProductItem ProductItem) : BaseDomainEvent
    {
    }
}
