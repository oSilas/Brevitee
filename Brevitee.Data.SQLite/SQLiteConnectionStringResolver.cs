using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace Brevitee.Data
{
    /// <summary>
    /// Resolves connection string requests to a sqlite database in the
    /// directory specified by the Directory property.
    /// </summary>
    public class SQLiteConnectionStringResolver: IConnectionStringResolver
    {
        public SQLiteConnectionStringResolver()
        {
            
        }

        static SQLiteConnectionStringResolver _instance;
        public static SQLiteConnectionStringResolver Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SQLiteConnectionStringResolver();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Register the default SQLiteConnectionStringResolver instance as 
        /// a ConnectionStringResolver
        /// </summary>
        public static void Register()
        {
            ConnectionStringResolvers.AddResolver(Instance);
        }

        /// <summary>
        /// The diretory to create the database in
        /// </summary>
        public DirectoryInfo Directory
        {
            get;
            set;
        }

        Func<DirectoryInfo> _directoryResolver;
        object _directoryResolverLock = new object();
        public Func<DirectoryInfo> DirectoryResolver
        {
            get
            {
                return _directoryResolverLock.DoubleCheckLock(ref _directoryResolver, () =>
                {
                    return () =>
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(".");
                        if (HttpContext.Current != null)
                        {
                            dirInfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/App_Data"));
                        }

                        return dirInfo;
                    };
                });
            }
        }

        #region IConnectionStringResolver Members

        public ConnectionStringSettings Resolve(string connectionName)
        {
            if (Directory == null)
            {
                Directory = DirectoryResolver();
                //if (HttpContext.Current != null)
                //{
                //    Directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/App_Data"));
                //}
                //else
                //{
                //    Directory = new DirectoryInfo(".");
                //}
            }

            ConnectionStringSettings s = new ConnectionStringSettings();
            s.ProviderName = SQLiteRegistrar.SQLiteAssemblyQualifiedName();
            string dbFile = Path.Combine(Directory.FullName, string.Format("{0}.sqlite", connectionName));
            s.ConnectionString = string.Format("Data Source={0};Version=3;", dbFile);

            return s;
        }

        #endregion
    }
}
