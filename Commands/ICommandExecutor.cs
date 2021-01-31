using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.Commands
{
    public interface ICommandExecutor
    {
        void Execute(TableCommand command);
    }
}
