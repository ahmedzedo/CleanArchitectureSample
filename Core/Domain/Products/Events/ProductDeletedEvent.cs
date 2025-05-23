﻿using CleanArchitecture.Domain.Common.Entities;
using CleanArchitecture.Domain.Products.Entites;
namespace CleanArchitecture.Domain.Products.Events
{
    public record ProductDeletedEvent(Product Product) : BaseDomainEvent
    {
    }
}
