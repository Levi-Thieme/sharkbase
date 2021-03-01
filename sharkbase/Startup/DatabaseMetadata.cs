using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.Startup
{
    public class DatabaseMetadata
    {
        public IEnumerable<string> TableNames { get; private set; }

        public DatabaseMetadata()
        {
            TableNames = new List<string>();
        }

        public DatabaseMetadata(IEnumerable<string> tableNames)
        {
            this.TableNames = tableNames;
        }
    }
}
