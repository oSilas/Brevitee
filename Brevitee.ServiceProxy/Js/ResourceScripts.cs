using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Yahoo.Yui.Compressor;

namespace Brevitee.ServiceProxy.Js
{
    public class ResourceScripts
    {
        volatile static Dictionary<string, string> _scripts;
        static List<Assembly> _loaded = new List<Assembly>();
        static object scriptLock = new object();
        static ResourceScripts()
        {
            _scripts = new Dictionary<string, string>();
            LoadScripts(typeof(ResourceScripts));
        }

        /// <summary>
        /// Loads all resource scripts in the namespace path of the specified type.
        /// </summary>
        /// <param name="type"></param>
        public static void LoadScripts(Type type)
        {
            lock (scriptLock)
            {
                Assembly assembly = type.Assembly;
                if (_loaded.Contains(assembly))
                {
                    return;
                }
                _loaded.Add(assembly);

                string namespacePath = string.Format("{0}.", type.Namespace);
                foreach (string fullScriptPath in assembly.GetManifestResourceNames())
                {
                    string scriptName = fullScriptPath.Substring(namespacePath.Length, fullScriptPath.Length - namespacePath.Length);
                    Stream resource = assembly.GetManifestResourceStream(fullScriptPath);
                    JavaScriptCompressor jsc = new JavaScriptCompressor();
                    using (StreamReader script = new StreamReader(resource))
                    {
                        string js = script.ReadToEnd();                        
                        _scripts.AddMissing(scriptName, js);
                        _scripts.AddMissing(
                            string.Format("{0}.min.js", scriptName.Substring(0, scriptName.Length - 3)),
                            jsc.Compress(js)
                        );
                    }
                }
            }
        }

        public static string Get(string scriptName, bool min = false)
        {
            string value = string.Empty;
            if (min && !scriptName.Trim().ToLowerInvariant().EndsWith("min.js"))
            {
                string minScript = string.Format("{0}.min.js", scriptName.Substring(0, scriptName.Length - 3));
                if (_scripts.ContainsKey(minScript))
                {
                    scriptName = minScript;
                }
            }

            if (_scripts.ContainsKey(scriptName))
            {
                value = _scripts[scriptName];
            }
            return value;
        }

        public static string Get(string scriptName, Type type)
        {
            LoadScripts(type);
            return Get(scriptName);
        }
    }
}
