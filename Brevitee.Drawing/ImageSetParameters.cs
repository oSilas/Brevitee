using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Naizari.Testing;
using System.IO;
//using Naizari.Data;
//using Naizari.Helpers;
using Brevitee;
using Brevitee.Testing;

namespace Brevitee.Images
{
    public class ImageSetParameters
    {
        Dictionary<string, string> parameters;
        public ImageSetParameters()
        {
            this.parameters = new Dictionary<string, string>();
        }

        object lockObj = new object();
        public string this[string parameterName]
        {
            get
            {
                lock (lockObj)
                {
                    if (this.parameters.ContainsKey(parameterName))
                        return this.parameters[parameterName];

                    return string.Empty;
                }
            }

            set
            {
                lock (lockObj)
                {
                    if (this.parameters.ContainsKey(parameterName))
                        this.parameters[parameterName] = value;
                    else
                        this.parameters.Add(parameterName, value);
                }
            }
        }

        //public string[] ParameterNames
        //{
        //    get
        //    {
        //        return DictionaryExtensions.KeysToArray<string, string>(this.parameters);
        //    }
        //}

        //public override string ToString()
        //{
        //    string ret = string.Empty;
        //    foreach (string parameterName in this.ParameterNames)
        //    {
        //        ret += parameterName + "=" + this[parameterName] + ";\r\n";
        //    }
        //    return ret;
        //}
    }
}
