using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevitee.Messaging
{
    public class NamedFormatEmailComposer: EmailComposer
    {
        public NamedFormatEmailComposer() : base() { }
        public NamedFormatEmailComposer(string templateDirectory, string extension = ".txt") : base(templateDirectory, extension) { }

        public override string GetEmailBody(string emailName, params object[] data)
        {
            Args.ThrowIfNull(data, "data");
            Args.ThrowIfNull(data[0], "data");

            object source = data[0];
            string templateContent = GetTemplateContent(emailName);

            return templateContent.NamedFormat(source);
        }
    }
}
