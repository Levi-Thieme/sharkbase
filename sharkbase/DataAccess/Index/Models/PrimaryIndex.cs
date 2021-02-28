using System.Collections.Generic;

namespace SharkBase.DataAccess.Index
{
    public class PrimaryIndex : Index<string, long>
    {
        public const string PRIMARY_INDEX_NAME = "PRIMARY";
        public PrimaryIndex(string table, Dictionary<string, long> indices) : base(table, IndexName(table), indices) { }
        public static string IndexName(string table) => $"{table}_{PRIMARY_INDEX_NAME}";
    }
}
