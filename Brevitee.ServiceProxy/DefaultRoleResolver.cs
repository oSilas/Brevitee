using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Brevitee.ServiceProxy
{
    public class DefaultRoleResolver: IRoleResolver
    {
        public bool IsInRole(IUserResolver userResolver, string roleName)
        {
            string userName = userResolver.GetCurrentUser();
            return Roles.IsUserInRole(userName, roleName);
        }

        public string[] GetRoles(IUserResolver userResolver)
        {
            return Roles.GetRolesForUser(userResolver.GetCurrentUser());
        }

        public IHttpContext HttpContext
        {
            get;
            set;
        }
    }
}
