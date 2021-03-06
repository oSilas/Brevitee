// Model is Table
using System;
using System.Data;
using System.Data.Common;
using Brevitee;
using Brevitee.Data;
using Brevitee.Data.Qi;

namespace Brevitee.Automation.ContinuousIntegration.Data
{
	// schema = BuildAutomation
	// connection Name = BuildAutomation
	[Brevitee.Data.Table("BuildResult", "BuildAutomation")]
	public partial class BuildResult: Dao
	{
		public BuildResult():base()
		{
			this.KeyColumnName = "Id";
			this.SetChildren();
		}

		public BuildResult(DataRow data): base(data)
		{
			this.KeyColumnName = "Id";
			this.SetChildren();
		}

		public static implicit operator BuildResult(DataRow data)
		{
			return new BuildResult(data);
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

	// property:Success, columnName:Success	
	[Brevitee.Data.Column(Name="Success", DbDataType="Bit", MaxLength="1", AllowNull=false)]
	public bool? Success
	{
		get
		{
			return GetBooleanValue("Success");
		}
		set
		{
			SetValue("Success", value);
		}
	}

	// property:Message, columnName:Message	
	[Brevitee.Data.Column(Name="Message", DbDataType="VarChar", MaxLength="4000", AllowNull=true)]
	public string Message
	{
		get
		{
			return GetStringValue("Message");
		}
		set
		{
			SetValue("Message", value);
		}
	}



	// start BuildJobId -> BuildJobId
	[Brevitee.Data.ForeignKey(
        Table="BuildResult",
		Name="BuildJobId", 
		DbDataType="BigInt", 
		MaxLength="",
		AllowNull=true, 
		ReferencedKey="Id",
		ReferencedTable="BuildJob",
		Suffix="1")]
	public long? BuildJobId
	{
		get
		{
			return GetLongValue("BuildJobId");
		}
		set
		{
			SetValue("BuildJobId", value);
		}
	}

	BuildJob _buildJobOfBuildJobId;
	public BuildJob BuildJobOfBuildJobId
	{
		get
		{
			if(_buildJobOfBuildJobId == null)
			{
				_buildJobOfBuildJobId = Brevitee.Automation.ContinuousIntegration.Data.BuildJob.OneWhere(f => f.Id == this.BuildJobId);
			}
			return _buildJobOfBuildJobId;
		}
	}
	
				
		

		/// <summary>
		/// Gets a query filter that should uniquely identify
		/// the current instance.  The default implementation
		/// compares the Id/key field to the current instance.
		/// </summary> 
		public override IQueryFilter GetUniqueFilter()
		{
			var colFilter = new BuildResultColumns();
			return (colFilter.Id == IdValue);
		}
		/// <summary>
		/// Return every record in the BuildResult table.
		/// </summary>
		/// <param name="database">
		/// The database to load from or null
		/// </param>
		public static BuildResultCollection LoadAll(Database database = null)
		{
			SqlStringBuilder sql = new SqlStringBuilder();
			sql.Select<BuildResult>();
			Database db = database == null ? Db.For<BuildResult>(): database;
			return new BuildResultCollection(sql.GetDataTable(db));
		}

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A Func delegate that recieves a BuildResultColumns 
		/// and returns a QueryFilter which is the result of any comparisons
		/// between BuildResultColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static BuildResultCollection Where(Func<BuildResultColumns, QueryFilter<BuildResultColumns>> where, OrderBy<BuildResultColumns> orderBy = null)
		{
			return new BuildResultCollection(new Query<BuildResultColumns, BuildResult>(where, orderBy), true);
		}
		
		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a BuildResultColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between BuildResultColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static BuildResultCollection Where(WhereDelegate<BuildResultColumns> where, Database db = null)
		{
			return new BuildResultCollection(new Query<BuildResultColumns, BuildResult>(where, db), true);
		}
		   
		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a BuildResultColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between BuildResultColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="db"></param>
		public static BuildResultCollection Where(WhereDelegate<BuildResultColumns> where, OrderBy<BuildResultColumns> orderBy = null, Database db = null)
		{
			return new BuildResultCollection(new Query<BuildResultColumns, BuildResult>(where, orderBy, db), true);
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of 
		/// one of the methods that take a delegate of type
		/// WhereDelegate<BuildResultColumns>.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="db"></param>
		public static BuildResultCollection Where(QiQuery where, Database db = null)
		{
			return new BuildResultCollection(Select<BuildResultColumns>.From<BuildResult>().Where(where, db));
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will 
		/// be thrown.  This method is most commonly used to retrieve a
		/// single BuildResult instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a BuildResultColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between BuildResultColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static BuildResult OneWhere(WhereDelegate<BuildResultColumns> where, Database db = null)
		{
			var results = new BuildResultCollection(Select<BuildResultColumns>.From<BuildResult>().Where(where, db));
			return OneOrThrow(results);
		}
			 
		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of 
		/// one of the methods that take a delegate of type
		/// WhereDelegate<BuildResultColumns>.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="db"></param>
		public static BuildResult OneWhere(QiQuery where, Database db = null)
		{
			var results = new BuildResultCollection(Select<BuildResultColumns>.From<BuildResult>().Where(where, db));
			return OneOrThrow(results);
		}

		private static BuildResult OneOrThrow(BuildResultCollection c)
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
		/// <param name="where">A WhereDelegate that recieves a BuildResultColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between BuildResultColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static BuildResult FirstOneWhere(WhereDelegate<BuildResultColumns> where, Database db = null)
		{
			var results = new BuildResultCollection(Select<BuildResultColumns>.From<BuildResult>().Where(where, db));
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
		/// <param name="where">A WhereDelegate that recieves a BuildResultColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between BuildResultColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static BuildResultCollection Top(int count, WhereDelegate<BuildResultColumns> where, Database db = null)
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
		/// <param name="where">A WhereDelegate that recieves a BuildResultColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between BuildResultColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="db"></param>
		public static BuildResultCollection Top(int count, WhereDelegate<BuildResultColumns> where, OrderBy<BuildResultColumns> orderBy, Database database = null)
		{
			BuildResultColumns c = new BuildResultColumns();
			IQueryFilter filter = where(c);         
			
			Database db = database == null ? Db.For<BuildResult>(): database;
			QuerySet query = GetQuerySet(db); 
			query.Top<BuildResult>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<BuildResultColumns>(orderBy);
			}

			query.Execute(db);
			return query.Results.As<BuildResultCollection>(0);
		}

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a BuildResultColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between BuildResultColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static long Count(WhereDelegate<BuildResultColumns> where, Database database = null)
		{
			BuildResultColumns c = new BuildResultColumns();
			IQueryFilter filter = where(c) ;

			Database db = database == null ? Db.For<BuildResult>(): database;
			QuerySet query = GetQuerySet(db);	 
			query.Count<BuildResult>();
			query.Where(filter);	  
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}
	}
}																								
