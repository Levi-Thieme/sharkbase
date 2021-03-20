using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.DataAccess;
using SharkBase.Models;
using SharkBase.Models.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharkBase.UnitTests.DataAccess
{
    [TestClass]
    public class TheRecord
    {
        private Record record;

        [TestInitialize]
        public void Initialize()
        {
            record = buildRecord();
        }

        private Record buildRecord()
        {
            var values = new List<Value>
            {
                new StringValue(Guid.Parse("6b7ad35b-8176-4139-9f60-fa5654412f81").ToString()),
                new BoolValue(false),
                new StringValue("Tacos"),
                new LongValue(6)
            };
            return new Record(values);
        }
    }
}
