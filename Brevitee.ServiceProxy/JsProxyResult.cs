using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using Brevitee;
using Brevitee.Data;
using Brevitee.Incubation;
using Yahoo.Yui.Compressor;

namespace Brevitee.ServiceProxy
{
    /// <summary>
    /// Used to render all registered javascript proxies to the client.
    /// </summary>
    public class JsProxyResult : JavaScriptResult
    {
        Incubator _serviceProvider = ServiceProxySystem.Incubator;
        public JsProxyResult(bool min)
        {
            this.Script = GetScript().ToString();
            if (min) Compress();
        }

        public JsProxyResult(string className, bool min)
        {
            this.Script = GetScript(className).ToString();
            if (min) Compress();
        }

        private void Compress()
        {
            JavaScriptCompressor jsc = new JavaScriptCompressor();
            this.Script = jsc.Compress(this.Script);
        }

        private StringBuilder GetScript()
        {
            return GetScript(ServiceProxyController.Incubator.ClassNames);
        }

        private StringBuilder GetScript(string className)
        {
            return GetScript(new string[] { className });
        }

        private StringBuilder GetScript(string[] classNames)
        {
            return ServiceProxySystem.GenerateJsProxyScript(_serviceProvider, classNames);
        }

        public string Ctors()
        {
            return DaoProxyRegistration.GetDaoJsCtorScript(_serviceProvider, _serviceProvider.ClassNames).ToString();
        }

        public string Proxies()
        {
            return ServiceProxySystem.GenerateJsProxyScript(_serviceProvider, _serviceProvider.ClassNames).ToString();
        }

    }
}
