using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.Analytics.Data
{
    public class PathCollection: DaoCollection<PathColumns, Path>
    { 
		public PathCollection(){}
		public PathCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public PathCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public PathCollection(Query<PathColumns, Path> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public PathCollection(Database db, Query<PathColumns, Path> q, bool load) : base(db, q, load) { }
		public PathCollection(Query<PathColumns, Path> q, bool load) : base(q, load) { }
    }
}