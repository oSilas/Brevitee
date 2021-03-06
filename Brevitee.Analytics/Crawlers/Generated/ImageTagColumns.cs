using System;
using System.Collections.Generic;
using System.Text;
using Brevitee.Data;

namespace Brevitee.Analytics.Data
{
    public class ImageTagColumns: QueryFilter<ImageTagColumns>, IFilterToken
    {
        public ImageTagColumns() { }
        public ImageTagColumns(string columnName)
            : base(columnName)
        { }
		
		public ImageTagColumns KeyColumn
		{
			get
			{
				return new ImageTagColumns("Id");
			}
		}	
				
﻿        public ImageTagColumns Id
        {
            get
            {
                return new ImageTagColumns("Id");
            }
        }

﻿        public ImageTagColumns ImageId
        {
            get
            {
                return new ImageTagColumns("ImageId");
            }
        }
﻿        public ImageTagColumns TagId
        {
            get
            {
                return new ImageTagColumns("TagId");
            }
        }

		protected internal Type TableType
		{
			get
			{
				return typeof(ImageTag);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}