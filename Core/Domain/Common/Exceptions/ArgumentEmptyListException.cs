namespace CleanArchitecture.Domain.Common.Exceptions
{
    public class ArgumentNullOrEmptyEnumerableException : ArgumentException
    {
        #region Constructor
        public ArgumentNullOrEmptyEnumerableException(string listArg)
          : base($"the {listArg} can not be empty Enumerable")
        {
        }

        public static void ThrowIfNullOrEmptyEnumerable<T>(IEnumerable<T> lst, string listDescrption)
        {
            if (lst is null || !lst.Any())
            {
                throw new ArgumentNullOrEmptyEnumerableException(listDescrption);
            }
        }
        #endregion
    }
}
