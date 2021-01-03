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
    public class OptionSetValuePrintableTests
    {
        private const int Value = 1;
        private OptionSetValue Default = new OptionSetValue(Value);
        private OptionSetValue Null = null;
        private const string CorrectFormatter = "1=Hello;2=World";
        private const string BrokenFormatter = "1Hello;;";
        private const string WrongFormatter = "0=Hello";

        [TestMethod()]
        public void ToString_SimpleTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.AreEqual(Value.ToString(), result);
        }

        [TestMethod()]
        public void ToString_SimpleNullTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Null);

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void ToString_CorrectFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default);

            //Act
            var result = Printer.ToString(CorrectFormatter);

            //Assert
            Assert.AreEqual("Hello", result);
        }

        [TestMethod()]
        public void ToString_BrokenFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default);

            //Act
            var result = Printer.ToString(BrokenFormatter);

            //Assert
            Assert.AreEqual(Value.ToString(), result);
        }

        [TestMethod()]
        public void ToString_WrongFormatTest()
        {
            //Arrange 
            var Printer = new OptionSetValuePrintable(Default);

            //Act
            var result = Printer.ToString(WrongFormatter);

            //Assert
            Assert.AreEqual(Value.ToString(), result);
        }
    }
}