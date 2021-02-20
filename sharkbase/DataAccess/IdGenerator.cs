using System;

namespace SharkBase.DataAccess
{
    public interface IGenerateId
    {
        Guid GetUniqueId();
    }
    public class IdGenerator : IGenerateId
    {
        public Guid GetUniqueId() => Guid.NewGuid();
    }
}
