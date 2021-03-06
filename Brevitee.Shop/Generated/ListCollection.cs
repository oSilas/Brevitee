using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.Shop
{
    public class ListCollection: DaoCollection<ListColumns, List>
    { 
		public ListCollection(){}
		public ListCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public ListCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public ListCollection(Query<ListColumns, List> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public ListCollection(Database db, Query<ListColumns, List> q, bool load) : base(db, q, load) { }
		public ListCollection(Query<ListColumns, List> q, bool load) : base(q, load) { }
    }
}