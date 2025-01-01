namespace CleanArchitecture.Domain.Common.Entities
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        DateTime? DeletedOnUtc { get; set; }
    }
}
