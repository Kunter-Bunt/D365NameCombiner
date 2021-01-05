using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;

namespace mwo.D365NameCombiner.Plugins.Decorators.Tests
{

    [TestClass]
    public class MoneyPrintableTests
    {

        private const decimal Value = 1.24M;
        private readonly Money Default = new Money(Value);
        private readonly Money Null = null;
        private const string SpecialFormatter = "C4";
        private const string SpecialFormatterExpected = "$1.2400";
        private const string Expected = "1.24";

        [TestMethod]
        public void ToString_SimpleTest()
        {
            //Arrange 
            var Printer = new MoneyPrintable(Default);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.AreEqual(Expected, result);
        }

        [TestMethod]
        public void ToString_NullTest()
        {
            //Arrange 
            var Printer = new MoneyPrintable(Null);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ToStringFormat_NullTest()
        {
            //Arrange 
            var Printer = new MoneyPrintable(Null);

            //Act
            var result = Printer.ToString(SpecialFormatter);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ToStringFormat_SimpleTest()
        {
            //Arrange 
            var Printer = new MoneyPrintable(Default);

            //Act
            var result = Printer.ToString(SpecialFormatter);

            //Assert
            Assert.AreEqual(SpecialFormatterExpected, result);
        }
    }
}