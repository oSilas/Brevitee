using System;
using System.Collections.Generic;
using System.Text;
using Brevitee.Data;

namespace Brevitee.Automation.ContinuousIntegration.Data
{
    public class BuildJobColumns: QueryFilter<BuildJobColumns>, IFilterToken
    {
        public BuildJobColumns() { }
        public BuildJobColumns(string columnName)
            : base(columnName)
        { }
		
		public BuildJobColumns KeyColumn
		{
			get
			{
				return new BuildJobColumns("Id");
			}
		}	
				
        public BuildJobColumns Id
        {
            get
            {
                return new BuildJobColumns("Id");
            }
        }
        public BuildJobColumns Name
        {
            get
            {
                return new BuildJobColumns("Name");
            }
        }
        public BuildJobColumns UserName
        {
            get
            {
                return new BuildJobColumns("UserName");
            }
        }
        public BuildJobColumns HostName
        {
            get
            {
                return new BuildJobColumns("HostName");
            }
        }
        public BuildJobColumns OutputPath
        {
            get
            {
                return new BuildJobColumns("OutputPath");
            }
        }


		protected internal Type TableType
		{
			get
			{
				return typeof(BuildJob);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}