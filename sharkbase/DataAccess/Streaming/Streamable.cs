using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.DataAccess.Streaming
{
    public interface Streamable<T> : IDisposable
    {
        T Current { get; }
        bool Read();
    }
}
