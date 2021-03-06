using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Brevitee.Configuration;
using Brevitee.Encryption;
using Brevitee.CommandLine;
using Brevitee.Incubation;
using Brevitee;
using Brevitee.Logging;
using Brevitee.Data;
using Brevitee.Testing;
using Brevitee.Javascript;
using Brevitee.Server;
using Brevitee.ServiceProxy;
using Brevitee.ServiceProxy.Secure;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Engines;
using FakeItEasy;
using FakeItEasy.Creation;

namespace Brevitee.ServiceProxy.Tests
{
    public partial class Program
    {
        class TestUserResolver: IUserResolver
        {

            public string GetCurrentUser()
            {
                return "TestUser";
            }

            public string GetUser(IHttpContext context)
            {
                return "TestUser";
            }

            public IHttpContext HttpContext
            {
                get;
                set;
            }
        }

        [UnitTest]
        public void UserResolver_ShouldResolveToTheSameAsUserUtil()
        {
            string user = UserUtil.GetCurrentWebUserName();
            string resolved = UserResolvers.Default.GetCurrentUser();

            Expect.AreEqual(user, resolved);
        }

        [UnitTest]
        public void UserResolver_ShouldResolveToTestUser()
        {
            UserResolvers.Default.Clear();
            UserResolvers.Default.AddResolver(new TestUserResolver());
            string userName = UserResolvers.Default.GetUser(A.Fake<IHttpContext>());
            Expect.AreEqual("TestUser", userName);
        }
    }
}
