using System;
using System.Collections.Generic;

namespace SharkBase.DataAccess.Streaming
{
    public interface Streamable<T> : IEnumerator<T>, IEnumerable<T>, IDisposable
    { }
}
