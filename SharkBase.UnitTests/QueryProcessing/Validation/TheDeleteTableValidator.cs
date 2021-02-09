using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharkBase.QueryProcessing.Validation;

namespace SharkBase.UnitTests.QueryProcessing.Validation
{
    [TestClass]
    public class TheDeleteTableValidator
    {
        [TestMethod]
        public void WhenValidating_DoesNotThrowAnException()
        {
            var validator = new DeleteTableValidator();
        }
    }
}
