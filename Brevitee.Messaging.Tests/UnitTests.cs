using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brevitee.CommandLine;
using Brevitee;
using Brevitee.Testing;
using Brevitee.Encryption;
using Brevitee.Messaging;
using Brevitee.Net;
using Brevitee.Net.Dns;
using System.Reflection;
using Brevitee.Encryption;
using System.IO;

namespace Brevitee.Messaging.Tests
{
    [Serializable]
    public class UnitTests : CommandLineTestInterface
    {
        static string _testTemplateName = "TestTemplate";
        static string _stringFormatContent = "this is the content: {0}";
        static string _namedFormatContent = "this is the named content: {Monkey}, {Banana}";

        static List<DirectoryInfo> _dirsToDelete;
        static UnitTests()
        {
            _dirsToDelete = new List<DirectoryInfo>();
        }

        public void CleanUp()
        {
            _dirsToDelete.Each(dir =>
            {
                dir.Delete(true);
            });
        }
               
        [UnitTest]
        public void ShouldBeAbleToSendEmail()
        {
            Vault stickerizeSettings = Vault.Load(new FileInfo(".\\StickerizeSmtpSettings.vault.sqlite"), "SmtpSettings");
            stickerizeSettings.Keys.Each(key =>
            {
                if (!key.ToLowerInvariant().Equals("password"))
                {
                    OutLineFormat("{0}={1}", key, stickerizeSettings[key]);
                }
            });
            Notify.ByEmail(stickerizeSettings, "bryan.apellanes@gmail.com").Subject("Test: " + MethodBase.GetCurrentMethod().Name).Body("This is a test from stickerize").Send();
        }

        [UnitTest(AlwaysAfter="CleanUp")]
        public void ShouldSetEmailTemplateInSpecifiedDirectory()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            EmailComposer composer = SetStringFormatComposerAndTestTemplate(methodName);

            string templatePath = Path.Combine(composer.TemplateDirectory.FullName, "TestTemplate.txt");
            FileInfo templateFile = new FileInfo(templatePath);
            Expect.IsTrue(templateFile.Exists, "template wasn't written");
        }

        [UnitTest(AlwaysAfter="CleanUp")]
        public void ShouldBeAbleToGetEmailBody()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            EmailComposer composer = SetStringFormatComposerAndTestTemplate(methodName);

            string body = composer.GetEmailBody(_testTemplateName, "Monkey");

            Expect.AreEqual(_stringFormatContent._Format("Monkey"), body);
        }

        [UnitTest(AlwaysAfter="CleanUp")]
        public void ShouldBeAbleToComposeEmail()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            EmailComposer composer = SetStringFormatComposerAndTestTemplate(methodName);

            string subject = "This is the Subject";
            Email email = composer.Compose(subject, _testTemplateName, "Baloney");

            Expect.IsNotNullOrEmpty(email.Config.Subject);
            Expect.IsNotNullOrEmpty(email.Config.Body);

            Expect.AreEqual(subject, email.Config.Subject);
            
            string body = _stringFormatContent._Format("Baloney");
            Expect.AreEqual(body, email.Config.Body);
        }


        [UnitTest(AlwaysAfter = "CleanUp")]
        public void ShouldBeAbleToGetNamedEmailBody()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            EmailComposer composer = SetNamedFormatComposerAndTestTemplate(methodName);
            object data = new { Monkey = "Corn", Banana = "Potato" };
            string body = composer.GetEmailBody(_testTemplateName, data);

            Expect.AreEqual(_namedFormatContent.NamedFormat(data), body);
        }

        [UnitTest(AlwaysAfter = "CleanUp")]
        public void ShouldBeAbleToComposeNamedEmail()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            EmailComposer composer = SetNamedFormatComposerAndTestTemplate(methodName);

            string subject = "This is the Subject";
            object data = new { Monkey = "Baloney", Banana = "Bananas" };
            Email email = composer.Compose(subject, _testTemplateName, data);

            Expect.IsNotNullOrEmpty(email.Config.Subject);
            Expect.IsNotNullOrEmpty(email.Config.Body);

            Expect.AreEqual(subject, email.Config.Subject);

            string body = _namedFormatContent.NamedFormat(data);
            Expect.AreEqual(body, email.Config.Body);
        }

        private static EmailComposer SetStringFormatComposerAndTestTemplate(string methodName)
        {
            string directoryName = ".\\{0}"._Format(methodName);

            EmailComposer composer = new StringFormatEmailComposer(directoryName);
            composer.SetEmailTemplate(_testTemplateName, _stringFormatContent);

            _dirsToDelete.Add(composer.TemplateDirectory);
            return composer;
        }

        private static EmailComposer SetNamedFormatComposerAndTestTemplate(string methodName)
        {
            string directoryName = ".\\{0}"._Format(methodName);

            EmailComposer composer = new NamedFormatEmailComposer(directoryName);
            composer.SetEmailTemplate(_testTemplateName, _namedFormatContent);
            
            _dirsToDelete.Add(composer.TemplateDirectory);
            return composer;
        }
    }
}
