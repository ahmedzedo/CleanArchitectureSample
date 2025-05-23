﻿namespace CleanArchitecture.Domain.Common.Entities
{
    public interface IAuditableEntity
    {
        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public string? LastUpdatedBy { get; set; }
    }
}
