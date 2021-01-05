using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Tests;

namespace mwo.D365NameCombiner.Plugins.Decorators.Tests
{
    [TestClass()]
    public class OptionSetValueCollectionPrintableTests : TestBase
    {
        private readonly OptionSetValueCollection Default = new OptionSetValueCollection
        {
            new OptionSetValue(ValueOne),
            new OptionSetValue(ValueTwo)
        };
        private readonly OptionSetValueCollection Null = null;
        private const string CorrectFormatter = "1=Hello;2=World";
        private const string CorrectFormatterWithSeparator = Separator + ";" + CorrectFormatter;
        private const string Separator = "Separator=-";
        private const string BrokenFormatter = "1Hello;;";
        private const string WrongFormatter = "0=Hello";

        [TestMethod()]
        public void ToString_SimpleTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.AreEqual($"{ValueOne} {ValueTwo}", result);
        }

        [TestMethod()]
        public void ToString_SimpleNullTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Null, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void ToString_CorrectFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(CorrectFormatter);

            //Assert
            Assert.AreEqual("Hello World", result);
        }

        [TestMethod()]
        public void ToString_CorrectFormatAndSepartorTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(CorrectFormatterWithSeparator);

            //Assert
            Assert.AreEqual("Hello-World", result);
        }
        [TestMethod()]
        public void ToString_SepartorTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(Separator);

            //Assert
            Assert.AreEqual($"{ValueOne}-{ValueTwo}", result);
        }

        [TestMethod()]
        public void ToString_BrokenFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(BrokenFormatter);

            //Assert
            Assert.AreEqual($"{ValueOne} {ValueTwo}", result);
        }

        [TestMethod()]
        public void ToString_WrongFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default, Context, EntityName, EnumsAttribute);

            //Act
            var result = Printer.ToString(WrongFormatter);

            //Assert
            Assert.AreEqual($"{ValueOne} {ValueTwo}", result);
        }
    }
}