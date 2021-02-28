using SharkBase.DataAccess.Index.Models;
using System.Collections.Generic;

namespace SharkBase.DataAccess.Index
{
    public class PrimaryIndex : Index<string, long>
    {
        public PrimaryIndex(string table, Dictionary<string, long> indices) : base(table, IndexName(table), indices) { }
        public static string IndexName(string table) => $"{table}_{IndexNames.PRIMARY}";
    }
}
