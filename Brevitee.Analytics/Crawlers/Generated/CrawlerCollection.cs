using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.Analytics.Data
{
    public class CrawlerCollection: DaoCollection<CrawlerColumns, Crawler>
    { 
		public CrawlerCollection(){}
		public CrawlerCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public CrawlerCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public CrawlerCollection(Query<CrawlerColumns, Crawler> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public CrawlerCollection(Database db, Query<CrawlerColumns, Crawler> q, bool load) : base(db, q, load) { }
		public CrawlerCollection(Query<CrawlerColumns, Crawler> q, bool load) : base(q, load) { }
    }
}