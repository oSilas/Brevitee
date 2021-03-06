using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevitee.Automation.ContinuousIntegration
{
    public class GetSourceException: Exception
    {
        public GetSourceException() : base("Failed to get source") { }

        public GetSourceException(string message) : base(message) { }

        public GetSourceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
