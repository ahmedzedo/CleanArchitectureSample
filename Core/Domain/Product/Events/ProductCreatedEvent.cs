using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Product.Events
{
    public record ProductCreatedEvent(Entites.Product Product) : BaseDomainEvent
    {

    }
}
