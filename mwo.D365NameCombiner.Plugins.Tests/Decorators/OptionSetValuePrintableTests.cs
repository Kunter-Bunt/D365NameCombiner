using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Tests;

namespace mwo.D365NameCombiner.Plugins.Decorators.Tests
{
    [TestClass()]
    public class OptionSetValuePrintableTests : TestBase
    {
        private OptionSetValue Default = new OptionSetValue(ValueOne);
        private OptionSetValue Null = null;
        private const string CorrectFormatter = "1=Hello;2=World";
        private const string BrokenFormatter = "1Hello;;";
        private const string WrongFormatter = "0=Hello";
        private const string LCIDFormatter = "LCID=1033";
        private const string LCIDWrongFormatter = "LCID=1031"; //german not configured for the optionset

        [TestMethod()]
        public void ToString_SimpleTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.AreEqual(ValueOne.ToString(), result);
        }

        [TestMethod()]
        public void ToString_SimpleNullTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Null, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void ToStringFormat_CorrectFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(CorrectFormatter);

            //Assert
            Assert.AreEqual("Hello", result);
        }

        [TestMethod()]
        public void ToStringFormat_BrokenFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(BrokenFormatter);

            //Assert
            Assert.AreEqual(ValueOne.ToString(), result);
        }

        [TestMethod()]
        public void ToStringFormat_WrongFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(WrongFormatter);

            //Assert
            Assert.AreEqual(ValueOne.ToString(), result);
        }

        [TestMethod()]
        public void ToStringFormat_CorrectLCIDTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(LCIDFormatter);

            //Assert
            Assert.AreEqual("Hello", result);
        }

        [TestMethod()]
        public void ToStringFormat_WrongLCIDTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(LCIDWrongFormatter);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void ToStringFormat_NoEnumTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default, Context, EntityName, NullAttribute);

            //Act
            var result = Printer.ToString(LCIDFormatter);

            //Assert
            Assert.IsNull(result);
        }
    }
}