using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CommonModels
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
