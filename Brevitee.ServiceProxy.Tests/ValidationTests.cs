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
        [UnitTest]
        public void Validation_ShouldBeAbleToCreateToken()
        {
            Prepare();
            
            IHttpContext context = CreateFakeContext(MethodBase.GetCurrentMethod().Name);
            SecureSession session = SecureSession.Get(context);
            string postString = ApiParameters.ParametersToJsonParamsObject("random information");

            ValidationToken token = ApiValidation.CreateValidationToken(postString, session);
        }

        public static void Prepare()
        {
            ConsoleLogger logger = new ConsoleLogger();
            SecureChannel.EnsureRepository(logger);
            RegisterDb();
            ClearApps();
        }

        [UnitTest]
        public void Validation_ShouldBeAbleToValidateToken()
        {
            Prepare();

            IHttpContext context = CreateFakeContext(MethodBase.GetCurrentMethod().Name);
            SecureSession session = SecureSession.Get(context);
            string postString = ApiParameters.ParametersToJsonParamsObject("random information");

            ValidationToken token = ApiValidation.CreateValidationToken(postString, session);

            Expect.AreEqual(TokenValidationStatus.Success, ApiValidation.ValidateToken(session, token, postString));
        }

        [UnitTest]
        public void Validation_ShouldBeAbleToSetAndValidateValidationToken()
        {
            Prepare();

            SecureSession session = SecureSession.Get(SecureSession.GenerateId());

            string postString = ApiParameters.ParametersToJsonParamsObject("random info");
            SecureServiceProxyClient<Echo> client = new SecureServiceProxyClient<Echo>("http://blah.com");

            HttpWebRequest request = client.GetServiceProxyRequest("Send");
            ApiValidation.SetValidationToken(request.Headers, postString, session.PublicKey);

            Cookie cookie = new Cookie(SecureSession.CookieName, session.Identifier, "", "blah.cxm");            
            request.CookieContainer.Add(cookie);
            request.Headers[SecureSession.CookieName] = session.Identifier;
        
            Expect.IsNotNull(request.Headers);
            Expect.IsNotNull(request.Headers[ApiValidation.NonceName]);
            Expect.IsNotNull(request.Headers[ApiValidation.ValidationTokenName]);

            Expect.AreEqual(TokenValidationStatus.Success, ApiValidation.ValidateToken(request.Headers, postString));
        }

        [UnitTest]
        public void Validation_ValidateNonceShouldFailIfTooOld()
        {
            Prepare();

            DateTime tenMinutesAgo = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(10));
            Instant nonce = new Instant(tenMinutesAgo);
            TokenValidationStatus status = ApiValidation.ValidateNonce(nonce.ToString(), 5);
            Expect.IsFalse(status == TokenValidationStatus.Success);
            Expect.AreEqual(TokenValidationStatus.NonceFailed, status);
        }
    }
}
