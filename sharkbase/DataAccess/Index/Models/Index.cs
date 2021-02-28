using System.Collections.Generic;
using System.Linq;

namespace SharkBase.DataAccess
{
    public class Index<K, V>
    {
        public readonly Dictionary<K, V> Indices;
        public string Table { get; private set; }
        public string Name { get; private set; }

        public Index(string table, string name, Dictionary<K, V> indices)
        {
            Table = table;
            Name = name;
            Indices = indices;
        }

        public void Add(K key, V value) => Indices.Add(key, value);

        public void Remove(K key) => Indices.Remove(key);

        public V GetValue(K key) => Indices[key];

        public bool HasKey(K key) => Indices.ContainsKey(key);

        public override bool Equals(object obj)
        {
            var other = obj as Index<K, V>;
            return other != null &&
                this.Name == other.Name &&
                this.Table == other.Table &&
                Enumerable.SequenceEqual(this.Indices, other.Indices);
        }

        public override int GetHashCode()
        {
            return Table.GetHashCode() + Name.GetHashCode() + Indices.GetHashCode();
        }
    }
}
