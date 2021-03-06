using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Brevitee.Data;

namespace Brevitee.Data.Tests
{
    public class CartItemCollection: DaoCollection<CartItemColumns, CartItem>
    { 
		public CartItemCollection(){}
		public CartItemCollection(Database db, DataTable table, Dao dao = null, string rc = null) : base(db, table, dao, rc) { }
		public CartItemCollection(DataTable table, Dao dao = null, string rc = null) : base(table, dao, rc) { }
		public CartItemCollection(Query<CartItemColumns, CartItem> q, Dao dao = null, string rc = null) : base(q, dao, rc) { }
		public CartItemCollection(Database db, Query<CartItemColumns, CartItem> q, bool load) : base(db, q, load) { }
		public CartItemCollection(Query<CartItemColumns, CartItem> q, bool load) : base(q, load) { }
    }
}