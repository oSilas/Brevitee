// Model is Table
using System;
using System.Data;
using System.Data.Common;
using Brevitee;
using Brevitee.Data;
using Brevitee.Data.Qi;

namespace Brevitee.BattleStickers
{
	// schema = BattleStickers
	// connection Name = BattleStickers
	[Brevitee.Data.Table("RequiredLevelSkill", "BattleStickers")]
	public partial class RequiredLevelSkill: Dao
	{
		public RequiredLevelSkill():base()
		{
			this.KeyColumnName = "Id";
			this.SetChildren();
		}

		public RequiredLevelSkill(DataRow data): base(data)
		{
			this.KeyColumnName = "Id";
			this.SetChildren();
		}

		public static implicit operator RequiredLevelSkill(DataRow data)
		{
			return new RequiredLevelSkill(data);
		}

		private void SetChildren()
		{
						
		}

	// property:Id, columnName:Id	
	[Exclude]
	[Brevitee.Data.KeyColumn(Name="Id", ExtractedType="", MaxLength="")]
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



	// start RequiredLevelId -> RequiredLevelId
	[Brevitee.Data.ForeignKey(
        Table="RequiredLevelSkill",
		Name="RequiredLevelId", 
		ExtractedType="", 
		MaxLength="",
		AllowNull=false, 
		ReferencedKey="Id",
		ReferencedTable="RequiredLevel",
		Suffix="1")]
	public long? RequiredLevelId
	{
		get
		{
			return GetLongValue("RequiredLevelId");
		}
		set
		{
			SetValue("RequiredLevelId", value);
		}
	}

	RequiredLevel _requiredLevelOfRequiredLevelId;
	public RequiredLevel RequiredLevelOfRequiredLevelId
	{
		get
		{
			if(_requiredLevelOfRequiredLevelId == null)
			{
				_requiredLevelOfRequiredLevelId = Brevitee.BattleStickers.RequiredLevel.OneWhere(f => f.Id == this.RequiredLevelId);
			}
			return _requiredLevelOfRequiredLevelId;
		}
	}
	
	// start SkillId -> SkillId
	[Brevitee.Data.ForeignKey(
        Table="RequiredLevelSkill",
		Name="SkillId", 
		ExtractedType="", 
		MaxLength="",
		AllowNull=false, 
		ReferencedKey="Id",
		ReferencedTable="Skill",
		Suffix="2")]
	public long? SkillId
	{
		get
		{
			return GetLongValue("SkillId");
		}
		set
		{
			SetValue("SkillId", value);
		}
	}

	Skill _skillOfSkillId;
	public Skill SkillOfSkillId
	{
		get
		{
			if(_skillOfSkillId == null)
			{
				_skillOfSkillId = Brevitee.BattleStickers.Skill.OneWhere(f => f.Id == this.SkillId);
			}
			return _skillOfSkillId;
		}
	}
	
				
		


		/// <summary>
		/// Gets a query filter that will should uniquely identify
		/// the current instance.  The default implementation
		/// compares the Id/key field to the current instance.
		/// </summary> 
		public override IQueryFilter GetUniqueFilter()
		{
			var colFilter = new RequiredLevelSkillColumns();
			return (colFilter.Id == IdValue);
		}

		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A Func delegate that recieves a RequiredLevelSkillColumns 
		/// and returns a QueryFilter which is the result of any comparisons
		/// between RequiredLevelSkillColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static RequiredLevelSkillCollection Where(Func<RequiredLevelSkillColumns, QueryFilter<RequiredLevelSkillColumns>> where, OrderBy<RequiredLevelSkillColumns> orderBy = null)
		{
			return new RequiredLevelSkillCollection(new Query<RequiredLevelSkillColumns, RequiredLevelSkill>(where, orderBy), true);
		}
		
		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RequiredLevelSkillColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequiredLevelSkillColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static RequiredLevelSkillCollection Where(WhereDelegate<RequiredLevelSkillColumns> where, Database db = null)
		{
			return new RequiredLevelSkillCollection(new Query<RequiredLevelSkillColumns, RequiredLevelSkill>(where, db), true);
		}
		   
		/// <summary>
		/// Execute a query and return the results. 
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RequiredLevelSkillColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequiredLevelSkillColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="db"></param>
		public static RequiredLevelSkillCollection Where(WhereDelegate<RequiredLevelSkillColumns> where, OrderBy<RequiredLevelSkillColumns> orderBy = null, Database db = null)
		{
			return new RequiredLevelSkillCollection(new Query<RequiredLevelSkillColumns, RequiredLevelSkill>(where, orderBy, db), true);
		}

		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of 
		/// one of the methods that take a delegate of type
		/// WhereDelegate<RequiredLevelSkillColumns>.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="db"></param>
		public static RequiredLevelSkillCollection Where(QiQuery where, Database db = null)
		{
			return new RequiredLevelSkillCollection(Select<RequiredLevelSkillColumns>.From<RequiredLevelSkill>().Where(where, db));
		}

		/// <summary>
		/// Execute a query that should return only one result.  If more
		/// than one result is returned a MultipleEntriesFoundException will 
		/// be thrown.  This method is most commonly used to retrieve a
		/// single RequiredLevelSkill instance by its Id/Key value
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RequiredLevelSkillColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequiredLevelSkillColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static RequiredLevelSkill OneWhere(WhereDelegate<RequiredLevelSkillColumns> where, Database db = null)
		{
			var results = new RequiredLevelSkillCollection(Select<RequiredLevelSkillColumns>.From<RequiredLevelSkill>().Where(where, db));
			return OneOrThrow(results);
		}
			 
		/// <summary>
		/// This method is intended to respond to client side Qi queries.
		/// Use of this method from .Net should be avoided in favor of 
		/// one of the methods that take a delegate of type
		/// WhereDelegate<RequiredLevelSkillColumns>.
		/// </summary>
		/// <param name="where"></param>
		/// <param name="db"></param>
		public static RequiredLevelSkill OneWhere(QiQuery where, Database db = null)
		{
			var results = new RequiredLevelSkillCollection(Select<RequiredLevelSkillColumns>.From<RequiredLevelSkill>().Where(where, db));
			return OneOrThrow(results);
		}

		private static RequiredLevelSkill OneOrThrow(RequiredLevelSkillCollection c)
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
		/// <param name="where">A WhereDelegate that recieves a RequiredLevelSkillColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequiredLevelSkillColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static RequiredLevelSkill FirstOneWhere(WhereDelegate<RequiredLevelSkillColumns> where, Database db = null)
		{
			var results = new RequiredLevelSkillCollection(Select<RequiredLevelSkillColumns>.From<RequiredLevelSkill>().Where(where, db));
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
		/// <param name="where">A WhereDelegate that recieves a RequiredLevelSkillColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequiredLevelSkillColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static RequiredLevelSkillCollection Top(int count, WhereDelegate<RequiredLevelSkillColumns> where, Database db = null)
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
		/// <param name="where">A WhereDelegate that recieves a RequiredLevelSkillColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequiredLevelSkillColumns and other values
		/// </param>
		/// <param name="orderBy">
		/// Specifies what column and direction to order the results by
		/// </param>
		/// <param name="db"></param>
		public static RequiredLevelSkillCollection Top(int count, WhereDelegate<RequiredLevelSkillColumns> where, OrderBy<RequiredLevelSkillColumns> orderBy, Database database = null)
		{
			RequiredLevelSkillColumns c = new RequiredLevelSkillColumns();
			IQueryFilter filter = where(c);         
			
			Database db = database == null ? _.Db.For<RequiredLevelSkill>(): database;
			QuerySet query = GetQuerySet(db); 
			query.Top<RequiredLevelSkill>(count);
			query.Where(filter);

			if(orderBy != null)
			{
				query.OrderBy<RequiredLevelSkillColumns>(orderBy);
			}

			query.Execute(db);
			return query.Results.As<RequiredLevelSkillCollection>(0);
		}

		/// <summary>
		/// Execute a query and return the number of results
		/// </summary>
		/// <param name="where">A WhereDelegate that recieves a RequiredLevelSkillColumns 
		/// and returns a IQueryFilter which is the result of any comparisons
		/// between RequiredLevelSkillColumns and other values
		/// </param>
		/// <param name="db"></param>
		public static long Count(WhereDelegate<RequiredLevelSkillColumns> where, Database database = null)
		{
			RequiredLevelSkillColumns c = new RequiredLevelSkillColumns();
			IQueryFilter filter = where(c) ;

			Database db = database == null ? _.Db.For<RequiredLevelSkill>(): database;
			QuerySet query = GetQuerySet(db);	 
			query.Count<RequiredLevelSkill>();
			query.Where(filter);	  
			query.Execute(db);
			return query.Results.As<CountResult>(0).Value;
		}
	}
}																								