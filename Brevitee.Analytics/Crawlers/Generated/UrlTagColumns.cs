using System;
using System.Collections.Generic;
using System.Text;
using Brevitee.Data;

namespace Brevitee.Analytics.Data
{
    public class UrlTagColumns: QueryFilter<UrlTagColumns>, IFilterToken
    {
        public UrlTagColumns() { }
        public UrlTagColumns(string columnName)
            : base(columnName)
        { }
		
		public UrlTagColumns KeyColumn
		{
			get
			{
				return new UrlTagColumns("Id");
			}
		}	
				
﻿        public UrlTagColumns Id
        {
            get
            {
                return new UrlTagColumns("Id");
            }
        }

﻿        public UrlTagColumns UrlId
        {
            get
            {
                return new UrlTagColumns("UrlId");
            }
        }
﻿        public UrlTagColumns TagId
        {
            get
            {
                return new UrlTagColumns("TagId");
            }
        }

		protected internal Type TableType
		{
			get
			{
				return typeof(UrlTag);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}