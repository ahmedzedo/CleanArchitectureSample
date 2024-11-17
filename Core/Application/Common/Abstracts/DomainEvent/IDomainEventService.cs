using CleanArchitecture.Domain.Common.Entities;

namespace CleanArchitecture.Application.Common.Abstracts.DomainEvent
{
    public interface IDomainEventService
    {
        Task Publish(BaseDomainEvent domainEvent);
    }
}
