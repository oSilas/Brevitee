using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Brevitee.Configuration
{
    public class InvalidIHasRequiredPropertiesImplementationException: Exception
    {
        public InvalidIHasRequiredPropertiesImplementationException(Type t, string propertyName)
            : base("The Type " + t.Name + " is an invalid implementation of IHasRequiredProperties.  The RequiredProperties property reported a non existent property on the object (" + propertyName + ").")
        { }
    }
}
