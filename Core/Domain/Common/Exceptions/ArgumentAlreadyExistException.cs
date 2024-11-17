using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CleanArchitecture.Domain.Common.Exceptions
{
    public class ArgumentAlreadyExistException : ArgumentException
    {
        #region Constructor
        public ArgumentAlreadyExistException(string arg, string source)
          : base($"the {arg} item is already exist in {source}")
        {
        }
        #endregion
    }
}
