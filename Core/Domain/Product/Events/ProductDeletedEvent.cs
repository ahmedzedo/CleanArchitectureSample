using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Product.Events
{
    public record ProductDeletedEvent(Entites.Product Product) : BaseDomainEvent
    {
    }
}
