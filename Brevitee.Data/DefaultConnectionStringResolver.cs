using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Brevitee.Data
{
    public class DefaultConnectionStringResolver: IConnectionStringResolver
    {
        static DefaultConnectionStringResolver _instance;
        static object _instanceLock = new object();
        public static DefaultConnectionStringResolver Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DefaultConnectionStringResolver();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// If specified, is used as the default connection string resolver
        /// </summary>
        public Func<string, System.Configuration.ConnectionStringSettings> Resolver
        {
            get;
            set;
        }

        #region IConnectionStringResolver Members

        public System.Configuration.ConnectionStringSettings Resolve(string connectionName)
        {
            if (Resolver != null)
            {
                return Resolver(connectionName);
            }
            else
            {
                return ConfigurationManager.ConnectionStrings[connectionName];
            }
        }

        #endregion
    }
}
