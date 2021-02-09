using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.QueryProcessing.Parsing
{
    public interface IParser
    {
        bool IsParsable(string input);
        IStatement Parse(string input);
    }
}
