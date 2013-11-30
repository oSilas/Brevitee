using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.StickerHeroes
{
    public class RequiredLevelSkillCollection: DaoCollection<RequiredLevelSkillColumns, RequiredLevelSkill>
    { 
		public RequiredLevelSkillCollection(){}
		public RequiredLevelSkillCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RequiredLevelSkillCollection(Query<RequiredLevelSkillColumns, RequiredLevelSkill> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RequiredLevelSkillCollection(Query<RequiredLevelSkillColumns, RequiredLevelSkill> q, bool load) : base(q, load) { }
    }
}