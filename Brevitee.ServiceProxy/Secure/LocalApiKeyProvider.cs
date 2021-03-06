using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brevitee.ServiceProxy;

namespace Brevitee.ServiceProxy.Secure
{
    public class LocalApiKeyProvider : ApiKeyProvider
    {
        public override string GetApplicationClientId(IApplicationNameProvider nameProvider)
        {
            return ApiKeyManager.Default.GetClientId(nameProvider);
        }

        public override string GetApplicationApiKey(string applicationClientId, int index)
        {
            return ApiKeyManager.Default.GetApplicationApiKey(applicationClientId, index);
        }
    }
}
