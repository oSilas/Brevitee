using System;
using System.Collections.Generic;
using System.Text;
using Brevitee.Data;

namespace Brevitee.UserAccounts.Data
{
    public class SessionColumns: QueryFilter<SessionColumns>, IFilterToken
    {
        public SessionColumns() { }
        public SessionColumns(string columnName)
            : base(columnName)
        { }
		
		public SessionColumns KeyColumn
		{
			get
			{
				return new SessionColumns("Id");
			}
		}	
				
        public SessionColumns Id
        {
            get
            {
                return new SessionColumns("Id");
            }
        }
        public SessionColumns Uuid
        {
            get
            {
                return new SessionColumns("Uuid");
            }
        }
        public SessionColumns Identifier
        {
            get
            {
                return new SessionColumns("Identifier");
            }
        }
        public SessionColumns CreationDate
        {
            get
            {
                return new SessionColumns("CreationDate");
            }
        }
        public SessionColumns LastActivity
        {
            get
            {
                return new SessionColumns("LastActivity");
            }
        }
        public SessionColumns IsActive
        {
            get
            {
                return new SessionColumns("IsActive");
            }
        }

        public SessionColumns UserId
        {
            get
            {
                return new SessionColumns("UserId");
            }
        }

		protected internal Type TableType
		{
			get
			{
				return typeof(Session);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}