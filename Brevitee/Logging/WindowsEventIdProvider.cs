using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brevitee.Logging
{
    public class WindowsEventIdProvider: EventIdProvider
    {
        public override int GetEventId(string applicationName, string messageSignature)
        {
            return 0;
        }
    }
}
