using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public class Record
    {
        public IEnumerable<object> Values { get; set; } = new List<object>();

        public Record(IEnumerable<object> values)
        {
            Values = values;
        }

        public Record(params object[] values)
        {
            Values = values;
        }
    }
}
