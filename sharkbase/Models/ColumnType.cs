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
        public const string StringType = "STRING";

        private static readonly IEnumerable<string> types = new List<string>
        {
            "INT64",
            StringType,
            "BOOLEAN"
        };

        public static IEnumerable<string> Types { get { return types; } }

        public static Dictionary<string, ColumnType> ColumnTypeByName = new Dictionary<string, ColumnType>
        {
            { "INT64", ColumnType.Int64 },
            { StringType, ColumnType.String },
            { "BOOLEAN", ColumnType.boolean }
        };

        public static bool Exists(string type) => types.Contains(type);
    }
}