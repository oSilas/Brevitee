using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.StickerHeroes
{
    public class PlayerTwoSkillCollection: DaoCollection<PlayerTwoSkillColumns, PlayerTwoSkill>
    { 
		public PlayerTwoSkillCollection(){}
		public PlayerTwoSkillCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public PlayerTwoSkillCollection(Query<PlayerTwoSkillColumns, PlayerTwoSkill> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public PlayerTwoSkillCollection(Query<PlayerTwoSkillColumns, PlayerTwoSkill> q, bool load) : base(q, load) { }
    }
}