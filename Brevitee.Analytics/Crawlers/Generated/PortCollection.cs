using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.Analytics.Data
{
    public class PortCollection: DaoCollection<PortColumns, Port>
    { 
		public PortCollection(){}
		public PortCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public PortCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public PortCollection(Query<PortColumns, Port> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public PortCollection(Database db, Query<PortColumns, Port> q, bool load) : base(db, q, load) { }
		public PortCollection(Query<PortColumns, Port> q, bool load) : base(q, load) { }
    }
}