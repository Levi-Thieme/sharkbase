using SharkBase.DataAccess.Index;
using System.Collections.Generic;
using System.IO;

namespace SharkBase.DataAccess
{
    public interface IndexRepository
    {
        void Upsert(PrimaryIndex index);
        void Upsert<K>(SecondaryIndex<K> index);
        PrimaryIndex Get(string name);
        SecondaryIndex<K> Get<K>(string name);
    }
}
