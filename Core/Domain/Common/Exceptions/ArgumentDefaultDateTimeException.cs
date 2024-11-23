namespace CleanArchitecture.Domain.Common.Exceptions
{
    public class ArgumentDefaultDateTimeException : ArgumentException
    {
        #region Constructor
        public ArgumentDefaultDateTimeException(DateTime dateTime)
          : base($"this date :{dateTime} is invalid date")
        {
        }
        #endregion

        public static void ThrowIfDateTimeIsDefault(DateTime dateTime)
        {
            if (dateTime == default)
            {
                throw new ArgumentDefaultDateTimeException(dateTime);
            }
        }

    }
}
