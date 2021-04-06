using SharkBase.DataAccess;
using SharkBase.Models.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.Parsing
{
    public interface IValueParser
    {
        public long ParseInt(string value);
        public string ParseString(string value);
        public bool ParseBoolean(string value);
        public Value ParseValue(string value, Column column);
        public IEnumerable<Value> ParseColumnValues(IEnumerable<string> columnValues, IEnumerable<Column> tableColumns);
    }

    public class ValueParser : IValueParser
    {
        public long ParseInt(string value)
        {
            return long.Parse(value);
        }

        public string ParseString(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        public bool ParseBoolean(string value)
        {
            return bool.Parse(value);
        }

        public Value ParseValue(string value, Column column)
        {
            if (column.Type == DataTypes.Int64)
                return new LongValue(ParseInt(value));
            else if (column.Type == DataTypes.String)
                return new StringValue(ParseString(value), column.Size);
            else if (column.Type == DataTypes.boolean)
                return new BoolValue(ParseBoolean(value));
            throw new ArgumentException("The column type did not correspond to an existing column type.");
        }

        public IEnumerable<Value> ParseColumnValues(IEnumerable<string> columnValues, IEnumerable<Column> tableColumns)
        {
            var values = new List<Value>();
            var parser = new ValueParser();
            foreach (var (column, value) in tableColumns.Zip(columnValues)) {
                try
                {
                    values.Add(parser.ParseValue(value, column));
                }
                catch (FormatException)
                {
                    throw new ArgumentException($"Unable to parse the column value, {value} given for column '{column.Name}' of type {column.Type}");
                }
            }
            return values;
        }
    }
}
