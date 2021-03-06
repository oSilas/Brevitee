using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Brevitee.Html
{
    /// <summary>
    /// The intended functionality of this class has 
    /// been superceded by the Webgrease bundling facility.
    /// 
    /// The intent with this class is to eventually create
    /// a mechanism that can optionally minify and combine all scripts
    /// for a page where this tag is used.  Currently this
    /// class just represents a script html tag/element.
    /// </summary>    
    public class JsTag: Tag
    {
        public JsTag()
            : base("script")
        {
            this.Attr("type", "text/javascript");
        }

        public JsTag(string path): this()
        {
            this.Path(path);
        }

        public JsTag Path(string path)
        {
            this.Attr("src", path);
            return this;
        }
    }
}
