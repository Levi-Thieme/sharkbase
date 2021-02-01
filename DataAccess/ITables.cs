using System.Collections.Generic;

namespace SharkBase.DataAccess
{
    public interface ITables
    {
        bool Exists(string name);
        void Create(string name, IEnumerable<Column> columns);
        void Delete(string name);
    }
}
