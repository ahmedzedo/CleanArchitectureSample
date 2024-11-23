using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Common.DependencyInjection.Exceptions
{
    public class NotFoundAssmblyException : Exception
    {
        #region Constructor
        public NotFoundAssmblyException(string assemblyName)
          : base($"Not found {{ {nameof(Application)} }} assembly or namespace in current app domain")
        {
        }
        #endregion
    }
}
