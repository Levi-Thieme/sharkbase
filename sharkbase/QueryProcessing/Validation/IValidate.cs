using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.QueryProcessing.Validation
{
    interface IValidate<T>
    {
        void Validate(T obj);
    }
}
