using System.Collections.Generic;

namespace SharkBase.DataAccess.Index
{
    public class SecondaryIndex<K> : Index<string, K>
    {
        public SecondaryIndex(string table, string name, Dictionary<string, K> indices) : base(table, name, indices) { }
    }
}
