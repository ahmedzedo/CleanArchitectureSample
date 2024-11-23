namespace CleanArchitecture.Domain.Common.Exceptions
{
    public class ArgumentNotExistException : ArgumentException
    {
        #region Constructor
        public ArgumentNotExistException(string arg, string source)
          : base($"the {arg} item is not exist in {source}")
        {
        }
        #endregion
    }
}
