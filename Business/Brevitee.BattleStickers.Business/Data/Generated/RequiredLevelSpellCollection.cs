using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.BattleStickers.Business.Data
{
    public class RequiredLevelSpellCollection: DaoCollection<RequiredLevelSpellColumns, RequiredLevelSpell>
    { 
		public RequiredLevelSpellCollection(){}
		public RequiredLevelSpellCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public RequiredLevelSpellCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RequiredLevelSpellCollection(Query<RequiredLevelSpellColumns, RequiredLevelSpell> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RequiredLevelSpellCollection(Database db, Query<RequiredLevelSpellColumns, RequiredLevelSpell> q, bool load) : base(db, q, load) { }
		public RequiredLevelSpellCollection(Query<RequiredLevelSpellColumns, RequiredLevelSpell> q, bool load) : base(q, load) { }
    }
}