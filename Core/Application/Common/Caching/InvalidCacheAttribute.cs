namespace CleanArchitecture.Application.Common.Caching
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class InvalidCacheAttribute : Attribute
    {
        #region Properties
        public required string KeyPrefix { get; set; }
        public CacheStore CacheStore { get; set; }
        #endregion
    }
}
