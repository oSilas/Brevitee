using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.Analytics.Metrics
{
    public class TimerCollection: DaoCollection<TimerColumns, Timer>
    { 
		public TimerCollection(){}
		public TimerCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public TimerCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public TimerCollection(Query<TimerColumns, Timer> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public TimerCollection(Database db, Query<TimerColumns, Timer> q, bool load) : base(db, q, load) { }
		public TimerCollection(Query<TimerColumns, Timer> q, bool load) : base(q, load) { }
    }
}