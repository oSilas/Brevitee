// Model is Table
using System;
using System.Data;
using System.Data.Common;
using Brevitee;
using Brevitee.Data;
using Brevitee.Data.Qi;

namespace Brevitee.UserAccounts.Data
{
	// schema = UserAccounts
	// connection Name = UserAccounts
	[Brevitee.Data.Table("Setting", "UserAccounts")]
	public partial class Setting: Dao
	{
		public Setting():base()
		{
			this.KeyColumnName = "Id";
			this.SetChildren();
		}

		public Setting(DataRow data): base(data)
		{
			this.KeyColumnName = "Id";
			this.SetChildren();
		}

		public static implicit operator Setting(DataRow data)
		{
			return new Setting(data);
		}

		private void SetChildren()
		{
						
		}

	// property:Id, columnName:Id	
	[Exclude]
	[Brevitee.Data.KeyColumn(Name="Id", DbDataType="BigInt", MaxLength="8")]
	public long? Id
	{
		get
		{
			return GetLongValue("Id");
		}
		set
		{
			SetValue("Id", value);
		}
	}

	// property:Uuid, columnName:Uuid	
	[Brevitee.Data.Column(Name="Uuid", DbDataType="VarChar", MaxLength="4000", AllowNull=false)]
	public string Uuid
	{
		get
		{
			return GetStringValue("Uuid");
		}
		set
		{
			SetValue("Uuid", value);
		}
	}

	// property:Key, columnName:Key	
	[Brevitee.Data.Column(Name="Key", DbDataType="VarChar", MaxLength="4000", AllowNull=false)]
	public string Key
	{
		get
		{
			return GetStringValue("Key");
		}
		set
		{
			SetValue("Key", value);
		}
	}

	// property:ValueType, columnName:ValueType	
	[Brevitee.Data.Column(Name="ValueType", DbDataType="VarChar", MaxLength="4000", AllowNull=false)]
	public string ValueType
	{
		get
		{
			return GetStringValue("ValueType");
		}
		set
		{
			SetValue("ValueType", value);
		}
	}

	// property:Value, columnName:Value	
	[Brevitee.Data.Column(Name="Value", DbDataType="VarBinary", MaxLength="8000", AllowNull=false)]
	public byte[] Value
	{
		get
		{
			return GetByteValue("Value");
		}
		set
		{
			SetValue("Value", value);
		}
	}



	// start UserId -> UserId
	[Brevitee.Data.ForeignKey(
        Table="Setting",
		Name="UserId", 
		DbDataType="BigInt", 
		MaxLength="",
		AllowNull=true, 
		ReferencedKey="Id",
		ReferencedTable="User",
		Suffix="1")]
	public long? UserId
	{
		get
		{
			return GetLongValue("UserId");
		}
		set
		{
			SetValue("UserId", value);
		}
	}

	User _userOfUserId;
	public User UserOfUserId
	{
		get
		{
			if(_userOfUserId == null)
			{
				_userOfUserId = Brevitee.UserAccounts.Data.User.OneWhere(f => f.Id == this.UserId);
			}
			return _userOfUserId;
		}
	}
	
				
		

		/// <summary>
		/// Gets a query filter that should uniquely identify
		/// the current instance.  The default implementation
		/// compares the Id/key field to the current instance.
		/// </summary> 
		public override IQueryFilter GetUniqueFilter()
		{
			var colFilter = new SettingColumns();
			return (colFilter.Id == IdValue);
		}
		/// <summary>
		/// Return every record in the Setting table.
		/// </summary>
		/// <param name="database">
		/// The database to load from or null
		/// </param>
		public static SettingCollection LoadAll(Database database = null)
		{
			SqlStringBuilder sql = new SqlStringBuilder();
			sql.Select<Setting>();
			Database db = database == null ? Db.For<Setting>(): database;
			var results = new SettingCollection(sql.GetDataTable(db));
			results.Database = db;
			return results;
		}

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A Func delegate that recieves a SettingColumns 
		/// and returns a QueryFilter which is the result of any comparisons
		/// between SettingColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static SettingCollection Where(Func<SettingColumns, QueryFilter<SettingColumns>> where, OrderBy<SettingColumns> orderBy = null)
		{
			return new SettingCollection(new Query<SettingColumns, Setting>(where, orderBy), true);
		}
		
		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a SettingColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between SettingColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static SettingCollection Where(WhereDelegate<SettingColumns> where, Database db = null)
		{
			var results = new SettingCollection(db, new Query<SettingColumns, Setting>(where, db), true);
			return results;
		}
		   
		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a SettingColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between SettingColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="db"></param>
		public static SettingCollection Where(WhereDelegate<SettingColumns> where, OrderBy<SettingColumns> orderBy = null, Database db = null)
		{
			var results = new SettingCollection(db, new Query<SettingColumns, Setting>(where, orderBy, db), true);
			return results;
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of 
		/// one of the methods that take a delegate of type
		/// WhereDelegate<SettingColumns>.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="db"></param>
		public static SettingCollection Where(QiQuery where, Database db = null)
		{
			var results = new SettingCollection(db, Select<SettingColumns>.From<Setting>().Where(where, db));
			return results;
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will 
		/// be thrown.  This method is most commonly used to retrieve a
		/// single Setting instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a SettingColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between SettingColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static Setting OneWhere(WhereDelegate<SettingColumns> where, Database db = null)
		{
			var results = new SettingCollection(db, Select<SettingColumns>.From<Setting>().Where(where, db));
			return OneOrThrow(results);
		}
			 
		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of 
		/// one of the methods that take a delegate of type
		/// WhereDelegate<SettingColumns>.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="db"></param>
		public static Setting OneWhere(QiQuery where, Database db = null)
		{
			var results = new SettingCollection(db, Select<SettingColumns>.From<Setting>().Where(where, db));
			return OneOrThrow(results);
		}

		private static Setting OneOrThrow(SettingCollection c)
		{
			if(c.Count == 1)
			{
				return c[0];
			}
			else if(c.Count > 1)
			{
				throw new MultipleEntriesFoundException();
			}

			return null;
		}

		/// <summary>
		/// Execute a query and return the first result
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a SettingColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between SettingColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static Setting FirstOneWhere(WhereDelegate<SettingColumns> where, Database db = null)
		{
			var results = new SettingCollection(db, Select<SettingColumns>.From<Setting>().Where(where, db));
			if(results.Count > 0)
			{
				return results[0];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Execute a query and return the specified number
		/// of values
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this 
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that recieves a SettingColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between SettingColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static SettingCollection Top(int count, WhereDelegate<SettingColumns> where, Database db = null)
		{
			return Top(count, where, null, db);
		}

		/// <summary>
		/// Execute a query and return the specified count
		/// of values
		/// </summary>
		/// <param name="count">The number of values to return.
		/// This value is used in the sql query so no more than this 
		/// number of values will be returned by the database.
		/// </param>
		/// <param name="where">A WhereDelegate that recieves a SettingColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between SettingColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="db"></param>
		public static SettingCollection Top(int count, WhereDelegate<SettingColumns> where, OrderBy<SettingColumns> orderBy, Database database = null)
		{
			SettingColumns c = new SettingColumns();
			IQueryFilter filter = where(c);         
			
			Database db = database == null ? Db.For<Setting>(): database;
			QuerySet query = GetQuerySet(db); 
			query.Top<Setting>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<SettingColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<SettingCollection>(0);
			results.Database = db;
			return results;
		}

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a SettingColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between SettingColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static long Count(WhereDelegate<SettingColumns> where, Database database = null)
		{
			SettingColumns c = new SettingColumns();
			IQueryFilter filter = where(c) ;

			Database db = database == null ? Db.For<Setting>(): database;
			QuerySet query = GetQuerySet(db);	 
			query.Count<Setting>();
			query.Where(filter);	  
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}
	}
}																								
