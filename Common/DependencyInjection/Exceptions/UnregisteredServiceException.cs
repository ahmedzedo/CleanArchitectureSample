namespace Common.DependencyInjection.Exceptions
{
    public class UnregisteredServiceException : Exception
    {
        public UnregisteredServiceException(string serviceName)
            : base($"no service for type {serviceName} has been registerd ")
        {
        }
    }
}
