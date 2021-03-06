using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevitee.Breve
{
    public abstract class BreveGenerator
    {
        public BreveGenerator(BreveInfo info)
        {
            this.Info = info;
        }

        public BreveInfo Info { get; private set; }

        public abstract void Go(string outputFile);

        public static BreveGenerator Create(Languages lang, BreveInfo info)
        {
            BreveGenerator generator = new JavaBreveGenerator(info);
            switch (lang)
            {
                case Languages.Invalid:
                    break;
                case Languages.cs:
                    generator = new CSharpBreveGenerator(info);
                    break;
                case Languages.java:
                    break;
                default:
                    break;
            }

            return generator;
        }
    }
}
