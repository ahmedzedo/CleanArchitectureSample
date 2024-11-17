namespace CleanArchitecture.Domain.Common.Entities
{
    public interface ISoftDeletable
    {
        public bool IsDelete { get; set; }
    }
}
