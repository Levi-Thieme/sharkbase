namespace SharkBase.DataAccess
{
    public class Column
    {
        public readonly DataTypes Type;
        public readonly string Name;
        public readonly bool HasDefaultValue;
        public readonly int Size;

        public Column(DataTypes type, string name, bool hasDefaultValue = false, int size = 0)
        {
            this.Type = type;
            this.Name = name;
            this.HasDefaultValue = hasDefaultValue;
            this.Size = size;
        }

        public override bool Equals(object obj)
        {
            if (obj is Column other)
            {
                return this.Type == other.Type && this.Name == other.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode() + this.Name.GetHashCode();
        }
    }
}
