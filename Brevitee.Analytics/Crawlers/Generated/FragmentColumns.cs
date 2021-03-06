using System;
using System.Collections.Generic;
using System.Text;
using Brevitee.Data;

namespace Brevitee.Analytics.Data
{
    public class FragmentColumns: QueryFilter<FragmentColumns>, IFilterToken
    {
        public FragmentColumns() { }
        public FragmentColumns(string columnName)
            : base(columnName)
        { }
		
		public FragmentColumns KeyColumn
		{
			get
			{
				return new FragmentColumns("Id");
			}
		}	
				
﻿        public FragmentColumns Id
        {
            get
            {
                return new FragmentColumns("Id");
            }
        }
﻿        public FragmentColumns Uuid
        {
            get
            {
                return new FragmentColumns("Uuid");
            }
        }
﻿        public FragmentColumns Value
        {
            get
            {
                return new FragmentColumns("Value");
            }
        }


		protected internal Type TableType
		{
			get
			{
				return typeof(Fragment);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}