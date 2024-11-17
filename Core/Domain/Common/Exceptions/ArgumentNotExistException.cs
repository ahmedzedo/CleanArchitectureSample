using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
