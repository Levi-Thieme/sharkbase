using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess
{
    public class Index
    {
        public Dictionary<string, long> Indices { get; private set; }
        public string Table { get; private set; }

        public Index(string table, Dictionary<string, long> indices)
        {
            Table = table;
            Indices = indices;
        }

        public void Add(string key, long value) => Indices.Add(key, value);

        public void Remove(string key) => Indices.Remove(key);

        public long GetValue(string key) => Indices[key];

        public bool HasKey(string key) => Indices.ContainsKey(key);
    }
}
