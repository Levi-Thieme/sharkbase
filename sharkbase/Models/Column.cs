namespace SharkBase.DataAccess
{
    public class Column
    {
        public readonly ColumnType Type;
        public readonly string Name;
        public readonly bool HasDefaultValue;

        public Column(ColumnType type, string name, bool hasDefaultValue = false)
        {
            this.Type = type;
            this.Name = name;
            this.HasDefaultValue = hasDefaultValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is Column)
            {
                var other = (Column)obj;
                return this.Type == other.Type &&
                    this.Name == other.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode() + this.Name.GetHashCode();
        }
    }
}
