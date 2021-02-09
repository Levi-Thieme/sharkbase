using System.Collections.Generic;

namespace SharkBase
{
    public interface IStatement
    {
        public string Table { get; set; }
        public IEnumerable<string> Tokens { get; set; }
        void Validate();
    }
}