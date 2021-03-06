using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brevitee.Messaging;
using Brevitee.ServiceProxy;
using Brevitee.Logging;
using System.IO;

namespace Brevitee.UserAccounts
{
    public class UserManagerConfig
    {
        public UserManagerConfig()
        {
            this.EmailTemplateDirectoryPath = ".\\EmailTemplates";
            this.EmailTemplateExtension = ".txt";
            this.EmailComposerType = typeof(NamedFormatEmailComposer).AssemblyQualifiedName;
            this.ApplicationNameResolverType = typeof(DefaultConfigurationApplicationNameProvider).AssemblyQualifiedName;
            this.SmtpSettingsVaultPath = ".\\SmtpSettings.vault.sqlite";
        }

        public UserManager Create(ILogger logger = null)
        {
            if (logger == null)
            {
                logger = Log.Default;
            }

            UserManager mgr = new UserManager();
            SetAppNameResolver(mgr, logger);
            SetEmailComposer(mgr, logger);

            if (!string.IsNullOrEmpty(SmtpSettingsVaultPath))
            {
                mgr.SmtpSettingsVaultPath = SmtpSettingsVaultPath;
            }

            return mgr;
        }
        
        public string ApplicationNameResolverType
        {
            get;
            set;
        }

        public string EmailComposerType
        {
            get;
            set;
        }

        public string EmailTemplateDirectoryPath
        {
            get;
            set;
        }

        public string EmailTemplateExtension
        {
            get;
            set;
        }

        public string SmtpSettingsVaultPath
        {
            get;
            set;
        }

        private void SetEmailComposer(UserManager mgr, ILogger logger)
        {
            Type emailComposerType = Type.GetType(EmailComposerType);
            if (emailComposerType == null)
            {
                logger.AddEntry("Specified EmailComposerType was not found, will use {0} instead: {1}", LogEventType.Warning, typeof(NamedFormatEmailComposer).Name, EmailComposerType);
            }
            else
            {
                EmailComposer composer = emailComposerType.Construct<EmailComposer>();
                composer.TemplateDirectory = new DirectoryInfo(EmailTemplateDirectoryPath);
                composer.FileExtension = EmailTemplateExtension;
                mgr.EmailComposer = composer;
                
                mgr.InitializeConfirmationEmail();
                mgr.InitializePasswordResetEmail();
            }
        }

        private void SetAppNameResolver(UserManager mgr, ILogger logger)
        {
            Type appNameResolverType = Type.GetType(ApplicationNameResolverType);
            if (appNameResolverType == null)
            {
                logger.AddEntry("Specified ApplicationNameResolverType was not found, will use {0} instead: {1}", LogEventType.Warning, typeof(DefaultConfigurationApplicationNameProvider).Name, ApplicationNameResolverType);
            }
            else
            {
                mgr.ApplicationNameResolver = appNameResolverType.Construct<IApplicationNameProvider>();
            }
        }
    }
}
