using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brevitee.Schema.Org
{
    public class Integer: Number
    {
        public Integer()
        {
            this.Name = "Integer";
        }

        public Integer(int value)
            : this()
        {
            this.Value = value;
        }
    }
}
