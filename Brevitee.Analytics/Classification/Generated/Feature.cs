// Model is Table
using System;
using System.Data;
using System.Data.Common;
using Brevitee;
using Brevitee.Data;
using Brevitee.Data.Qi;

namespace Brevitee.Analytics.Classification
{
	// schema = Classification
	// connection Name = Classification
	[Brevitee.Data.Table("Feature", "Classification")]
	public partial class Feature: Dao
	{
		public Feature():base()
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public Feature(DataRow data): base(data)
		{
			this.SetKeyColumnName();
			this.SetChildren();
		}

		public static implicit operator Feature(DataRow data)
		{
			return new Feature(data);
		}

		private void SetChildren()
		{
						
		}

﻿	// property:Id, columnName:Id	
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

﻿	// property:Uuid, columnName:Uuid	
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

﻿	// property:Value, columnName:Value	
	[Brevitee.Data.Column(Name="Value", DbDataType="VarChar", MaxLength="4000", AllowNull=false)]
	public string Value
	{
		get
		{
			return GetStringValue("Value");
		}
		set
		{
			SetValue("Value", value);
		}
	}

﻿	// property:FeatureToCategoryCount, columnName:FeatureToCategoryCount	
	[Brevitee.Data.Column(Name="FeatureToCategoryCount", DbDataType="BigInt", MaxLength="8", AllowNull=false)]
	public long? FeatureToCategoryCount
	{
		get
		{
			return GetLongValue("FeatureToCategoryCount");
		}
		set
		{
			SetValue("FeatureToCategoryCount", value);
		}
	}



﻿	// start CategoryId -> CategoryId
	[Brevitee.Data.ForeignKey(
        Table="Feature",
		Name="CategoryId", 
		DbDataType="BigInt", 
		MaxLength="",
		AllowNull=true, 
		ReferencedKey="Id",
		ReferencedTable="Category",
		Suffix="1")]
	public long? CategoryId
	{
		get
		{
			return GetLongValue("CategoryId");
		}
		set
		{
			SetValue("CategoryId", value);
		}
	}

	Category _categoryOfCategoryId;
	public Category CategoryOfCategoryId
	{
		get
		{
			if(_categoryOfCategoryId == null)
			{
				_categoryOfCategoryId = Brevitee.Analytics.Classification.Category.OneWhere(c => c.KeyColumn == this.CategoryId);
			}
			return _categoryOfCategoryId;
		}
	}
	
				
		

		/// <summary>
		/// Gets a query filter that should uniquely identify
		/// the current instance.  The default implementation
		/// compares the Id/key field to the current instance.
		/// </summary> 
		public override IQueryFilter GetUniqueFilter()
		{
			var colFilter = new FeatureColumns();
			return (colFilter.KeyColumn == IdValue);
		}
		/// <summary>
		/// Return every record in the Feature table.
		/// </summary>
		/// <param name="database">
		/// The database to load from or null
		/// </param>
		public static FeatureCollection LoadAll(Database database = null)
		{
			SqlStringBuilder sql = new SqlStringBuilder();
			sql.Select<Feature>();
			Database db = database ?? Db.For<Feature>();
			var results = new FeatureCollection(sql.GetDataTable(db));
			results.Database = db;
			return results;
		}

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A Func delegate that recieves a FeatureColumns 
		/// and returns a QueryFilter which is the result of any comparisons
		/// between FeatureColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static FeatureCollection Where(Func<FeatureColumns, QueryFilter<FeatureColumns>> where, OrderBy<FeatureColumns> orderBy = null)
		{
			return new FeatureCollection(new Query<FeatureColumns, Feature>(where, orderBy), true);
		}
		
		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a FeatureColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between FeatureColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static FeatureCollection Where(WhereDelegate<FeatureColumns> where, Database db = null)
		{
			var results = new FeatureCollection(db, new Query<FeatureColumns, Feature>(where, db), true);
			return results;
		}
		   
		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a FeatureColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between FeatureColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="db"></param>
		public static FeatureCollection Where(WhereDelegate<FeatureColumns> where, OrderBy<FeatureColumns> orderBy = null, Database db = null)
		{
			var results = new FeatureCollection(db, new Query<FeatureColumns, Feature>(where, orderBy, db), true);
			return results;
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of 
		/// one of the methods that take a delegate of type
		/// WhereDelegate<FeatureColumns>.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="db"></param>
		public static FeatureCollection Where(QiQuery where, Database db = null)
		{
			var results = new FeatureCollection(db, Select<FeatureColumns>.From<Feature>().Where(where, db));
			return results;
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will 
		/// be thrown.  This method is most commonly used to retrieve a
		/// single Feature instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a FeatureColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between FeatureColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static Feature OneWhere(WhereDelegate<FeatureColumns> where, Database db = null)
		{
			var results = new FeatureCollection(db, Select<FeatureColumns>.From<Feature>().Where(where, db));
			return OneOrThrow(results);
		}
			 
		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of 
		/// one of the methods that take a delegate of type
		/// WhereDelegate<FeatureColumns>.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="db"></param>
		public static Feature OneWhere(QiQuery where, Database db = null)
		{
			var results = new FeatureCollection(db, Select<FeatureColumns>.From<Feature>().Where(where, db));
			return OneOrThrow(results);
		}

		private static Feature OneOrThrow(FeatureCollection c)
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
		/// <param name="where">A WhereDelegate that recieves a FeatureColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between FeatureColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static Feature FirstOneWhere(WhereDelegate<FeatureColumns> where, Database db = null)
		{
			var results = new FeatureCollection(db, Select<FeatureColumns>.From<Feature>().Where(where, db));
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
		/// <param name="where">A WhereDelegate that recieves a FeatureColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between FeatureColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static FeatureCollection Top(int count, WhereDelegate<FeatureColumns> where, Database db = null)
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
		/// <param name="where">A WhereDelegate that recieves a FeatureColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between FeatureColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="db"></param>
		public static FeatureCollection Top(int count, WhereDelegate<FeatureColumns> where, OrderBy<FeatureColumns> orderBy, Database database = null)
		{
			FeatureColumns c = new FeatureColumns();
			IQueryFilter filter = where(c);         
			
			Database db = database == null ? Db.For<Feature>(): database;
			QuerySet query = GetQuerySet(db); 
			query.Top<Feature>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<FeatureColumns>(orderBy);
			}

			query.Execute(db);
			var results = query.Results.As<FeatureCollection>(0);
			results.Database = db;
			return results;
		}

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a FeatureColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between FeatureColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static long Count(WhereDelegate<FeatureColumns> where, Database database = null)
		{
			FeatureColumns c = new FeatureColumns();
			IQueryFilter filter = where(c) ;

			Database db = database == null ? Db.For<Feature>(): database;
			QuerySet query = GetQuerySet(db);	 
			query.Count<Feature>();
			query.Where(filter);	  
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}
	}
}																								
