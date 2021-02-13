using SharkBase.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharkBase.Parsing
{
    public interface IValueParser
    {
        public long ParseInt(string value);
        public string ParseString(string value);
        public object ParseValue(string value, ColumnType type);
        public IEnumerable<object> ParseColumnValues(IEnumerable<string> columnValues, IEnumerable<Column> tableColumns);
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

        public object ParseValue(string value, ColumnType type)
        {
            if (type == ColumnType.Int64)
                return ParseInt(value);
            else if (type == ColumnType.String)
                return ParseString(value);
            throw new ArgumentException("The column type did not correspond to an existing column type.");
        }

        public IEnumerable<object> ParseColumnValues(IEnumerable<string> columnValues, IEnumerable<Column> tableColumns)
        {
            var values = new List<object>();
            var parser = new ValueParser();
            for (int i = 0; i < tableColumns.Count(); i++)
            {
                string value = columnValues.ElementAt(i);
                var column = tableColumns.ElementAt(i);
                try
                {
                    values.Add(parser.ParseValue(value, column.Type));
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
