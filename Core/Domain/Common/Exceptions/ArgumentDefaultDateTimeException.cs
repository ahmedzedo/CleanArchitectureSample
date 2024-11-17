using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
