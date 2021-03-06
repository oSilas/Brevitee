using System;
using System.Collections.Generic;
using System.Text;
using Brevitee.Data;

namespace Brevitee.Automation.ContinuousIntegration.Data
{
    public class BuildResultColumns: QueryFilter<BuildResultColumns>, IFilterToken
    {
        public BuildResultColumns() { }
        public BuildResultColumns(string columnName)
            : base(columnName)
        { }
		
		public BuildResultColumns KeyColumn
		{
			get
			{
				return new BuildResultColumns("Id");
			}
		}	
				
        public BuildResultColumns Id
        {
            get
            {
                return new BuildResultColumns("Id");
            }
        }
        public BuildResultColumns Success
        {
            get
            {
                return new BuildResultColumns("Success");
            }
        }
        public BuildResultColumns Message
        {
            get
            {
                return new BuildResultColumns("Message");
            }
        }

        public BuildResultColumns BuildJobId
        {
            get
            {
                return new BuildResultColumns("BuildJobId");
            }
        }

		protected internal Type TableType
		{
			get
			{
				return typeof(BuildResult);
			}
		}

		public string Operator { get; set; }

        public override string ToString()
        {
            return base.ColumnName;
        }
	}
}