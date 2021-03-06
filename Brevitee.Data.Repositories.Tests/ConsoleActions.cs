using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Brevitee.CommandLine;
using Brevitee;
using Brevitee.Testing;
using Brevitee.Encryption;

namespace Brevitee.Repositories.Tests
{
    [Serializable]
    public class ConsoleActions : CommandLineTestInterface
    {
        // See the below for examples of ConsoleActions and UnitTests

        #region ConsoleAction examples
        [ConsoleAction("Non Static Test")]
        public void NonStatic()
        {
            Pass("Test passed");
        }

        [ConsoleAction("Static Test")]
        public static void StaticTest()
        {
            Pass("Test passed");
        }

        [ConsoleAction("With Parameters")]
        public static void WithParameters(string inputString)
        {
            OutFormat("You typed {0}", inputString);
        }

        [ConsoleAction("Warn")]
        public void DoWarn()
        {
            Warn("This is a warning message");
        }

        [ConsoleAction("Error")]
        public void DoError()
        {
            Error("This is an error", new Exception("This is an exception"));
        }
        #endregion
    }
}