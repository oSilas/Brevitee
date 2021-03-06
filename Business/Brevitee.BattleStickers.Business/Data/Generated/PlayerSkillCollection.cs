using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.BattleStickers.Business.Data
{
    public class PlayerSkillCollection: DaoCollection<PlayerSkillColumns, PlayerSkill>
    { 
		public PlayerSkillCollection(){}
		public PlayerSkillCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public PlayerSkillCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public PlayerSkillCollection(Query<PlayerSkillColumns, PlayerSkill> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public PlayerSkillCollection(Database db, Query<PlayerSkillColumns, PlayerSkill> q, bool load) : base(db, q, load) { }
		public PlayerSkillCollection(Query<PlayerSkillColumns, PlayerSkill> q, bool load) : base(q, load) { }
    }
}