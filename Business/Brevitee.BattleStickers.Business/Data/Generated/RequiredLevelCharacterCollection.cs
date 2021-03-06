using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.BattleStickers.Business.Data
{
    public class RequiredLevelCharacterCollection: DaoCollection<RequiredLevelCharacterColumns, RequiredLevelCharacter>
    { 
		public RequiredLevelCharacterCollection(){}
		public RequiredLevelCharacterCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public RequiredLevelCharacterCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public RequiredLevelCharacterCollection(Query<RequiredLevelCharacterColumns, RequiredLevelCharacter> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public RequiredLevelCharacterCollection(Database db, Query<RequiredLevelCharacterColumns, RequiredLevelCharacter> q, bool load) : base(db, q, load) { }
		public RequiredLevelCharacterCollection(Query<RequiredLevelCharacterColumns, RequiredLevelCharacter> q, bool load) : base(q, load) { }
    }
}