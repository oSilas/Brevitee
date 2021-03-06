using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.BattleStickers.Business.Data
{
    public class PlayerTwoCharacterCollection: DaoCollection<PlayerTwoCharacterColumns, PlayerTwoCharacter>
    { 
		public PlayerTwoCharacterCollection(){}
		public PlayerTwoCharacterCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public PlayerTwoCharacterCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public PlayerTwoCharacterCollection(Query<PlayerTwoCharacterColumns, PlayerTwoCharacter> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public PlayerTwoCharacterCollection(Database db, Query<PlayerTwoCharacterColumns, PlayerTwoCharacter> q, bool load) : base(db, q, load) { }
		public PlayerTwoCharacterCollection(Query<PlayerTwoCharacterColumns, PlayerTwoCharacter> q, bool load) : base(q, load) { }
    }
}