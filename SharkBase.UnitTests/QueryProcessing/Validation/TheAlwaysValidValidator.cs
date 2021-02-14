using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.QueryProcessing.Validation;

namespace SharkBase.UnitTests.QueryProcessing.Validation
{
    [TestClass]
    public class TheAlwaysValidValidator
    {
        [TestMethod]
        public void IsAlwaysValid()
        {
            var validator = new AlwaysValidValidator();

            validator.Validate(null, null);
        }
    }
}
