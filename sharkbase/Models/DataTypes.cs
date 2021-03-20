using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.DataAccess
{
    public enum DataTypes
    {
        Int64,
        String,
        boolean
    }

    public class ColumnTypes
    {
        public const string StringType = "STRING";
        public const string Int64Type = "INT64";
        public const string BooleanType = "BOOLEAN";

        private static readonly IEnumerable<string> types = new List<string>
        {
            Int64Type,
            StringType,
            BooleanType
        };

        public static IEnumerable<string> Types { get { return types; } }

        public static Dictionary<string, DataTypes> ColumnTypeByName = new Dictionary<string, DataTypes>
        {
            { Int64Type, DataTypes.Int64 },
            { StringType, DataTypes.String },
            { BooleanType, DataTypes.boolean }
        };

        public static bool Exists(string type) => types.Contains(type);
    }
}