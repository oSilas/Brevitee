using System;
using System.Collections.Generic;
using System.Text;
using Brevitee.Data;

namespace Brevitee.Analytics.Metrics
{
    public class CustomTimerColumns: QueryFilter<CustomTimerColumns>, IFilterToken
    {
        public CustomTimerColumns() { }
        public CustomTimerColumns(string columnName)
            : base(columnName)
        { }
		
		public CustomTimerColumns KeyColumn
		{
			get
			{
				return new CustomTimerColumns("Id");
			}
		}	
				
﻿        public CustomTimerColumns Id
        {
            get
            {
                return new CustomTimerColumns("Id");
            }
        }
﻿        public CustomTimerColumns Uuid
        {
            get
            {
                return new CustomTimerColumns("Uuid");
            }
        }
﻿        public CustomTimerColumns Name
        {
            get
            {
                return new CustomTimerColumns("Name");
            }
        }
﻿        public CustomTimerColumns Description
        {
            get
            {
                return new CustomTimerColumns("Description");
            }
        }

﻿        public CustomTimerColumns TimerId
        {
            get
            {
                return new CustomTimerColumns("TimerId");
            }
        }

		protected internal Type TableType
		{
			get
			{
				return typeof(CustomTimer);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}