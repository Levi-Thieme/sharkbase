using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.DataAccess
{
    public enum ColumnType
    {
        Int64,
        String,
        boolean
    }

    public class ColumnTypes
    {
        private static readonly IEnumerable<string> types = new List<string>
        {
            "INT64",
            "STRING",
            "BOOLEAN"
        };

        public static IEnumerable<string> Types { get { return types; } }

        public static Dictionary<string, ColumnType> ColumnTypeByName = new Dictionary<string, ColumnType>
        {
            { "INT64", ColumnType.Int64 },
            { "STRING", ColumnType.String },
            { "BOOLEAN", ColumnType.boolean }
        };

        public static bool Exists(string type) => types.Contains(type);
    }
}