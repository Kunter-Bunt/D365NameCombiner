using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwo.D365NameCombiner.Plugins.Decorators.Tests
{
    [TestClass()]
    public class OptionSetValueCollectionPrintableTests
    {
        private const int ValueOne = 1;
        private const int ValueTwo = 2;
        private OptionSetValueCollection Default = new OptionSetValueCollection
        {
            new OptionSetValue(ValueOne),
            new OptionSetValue(ValueTwo)
        };
        private OptionSetValueCollection Null = null;
        private const string CorrectFormatter = "1=Hello;2=World";
        private const string CorrectFormatterWithSeparator = Separator + ";" +  CorrectFormatter;
        private const string Separator = "Separator=-";
        private const string BrokenFormatter = "1Hello;;";
        private const string WrongFormatter = "0=Hello";

        [TestMethod()]
        public void ToString_SimpleTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.AreEqual($"{ValueOne} {ValueTwo}", result);
        }

        [TestMethod()]
        public void ToString_SimpleNullTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Null);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void ToString_CorrectFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default);

            //Act
            var result = Printer.ToString(CorrectFormatter);

            //Assert
            Assert.AreEqual("Hello World", result);
        }

        [TestMethod()]
        public void ToString_CorrectFormatAndSepartorTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default);

            //Act
            var result = Printer.ToString(CorrectFormatterWithSeparator);

            //Assert
            Assert.AreEqual("Hello-World", result);
        }
        [TestMethod()]
        public void ToString_SepartorTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default);

            //Act
            var result = Printer.ToString(Separator);

            //Assert
            Assert.AreEqual($"{ValueOne}-{ValueTwo}", result);
        }

        [TestMethod()]
        public void ToString_BrokenFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default);

            //Act
            var result = Printer.ToString(BrokenFormatter);

            //Assert
            Assert.AreEqual($"{ValueOne} {ValueTwo}", result);
        }

        [TestMethod()]
        public void ToString_WrongFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValueCollectionPrintable(Default);

            //Act
            var result = Printer.ToString(WrongFormatter);

            //Assert
            Assert.AreEqual($"{ValueOne} {ValueTwo}", result);
        }
    }
}