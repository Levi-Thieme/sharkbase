using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharkBase.Models.Values
{
    public interface Readable
    {
        void Read(BinaryReader reader);
    }
}
