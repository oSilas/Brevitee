using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brevitee.Logging
{
    public class EventIdProvider
    {
        public virtual int GetEventId(string applicationName, string messageSignature)
        {
            return (applicationName + messageSignature).GetHashCode();
        }
    }
}
