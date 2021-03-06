using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.Analytics.Data
{
    public class UrlCollection: DaoCollection<UrlColumns, Url>
    { 
		public UrlCollection(){}
		public UrlCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public UrlCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public UrlCollection(Query<UrlColumns, Url> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public UrlCollection(Database db, Query<UrlColumns, Url> q, bool load) : base(db, q, load) { }
		public UrlCollection(Query<UrlColumns, Url> q, bool load) : base(q, load) { }
    }
}