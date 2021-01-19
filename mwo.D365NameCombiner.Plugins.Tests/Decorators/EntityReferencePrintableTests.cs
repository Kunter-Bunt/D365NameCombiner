using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Services;
using mwo.D365NameCombiner.Plugins.Tests;

namespace mwo.D365NameCombiner.Plugins.Decorators.Tests
{
    [TestClass]
    public class EntityReferencePrintableTests : TestBase
    {
        [TestMethod]
        public void ToString_SimpleTest()
        {
            //Arrange 
            var entref = Target.GetAttributeValue<EntityReference>(LookupAttribute);
            var Printer = new EntityReferencePrintable(entref, Context, new AttributeConverterService(Context));

            //Act
            var result = Printer.ToString();

            //Assert
            StringAssert.Contains(result, entref.LogicalName);
            StringAssert.Contains(result, entref.Id.ToString());
        }

        [TestMethod]
        public void ToString_NullTest()
        {
            //Arrange 
            var entref = Target.GetAttributeValue<EntityReference>(NullAttribute);
            var Printer = new EntityReferencePrintable(entref, Context, new AttributeConverterService(Context));

            //Act
            var result = Printer.ToString();

            //Assert
            Assert.IsNull(result);
        }

        [DataTestMethod]
        [DataRow("Name", LookupValueName)]
        [DataRow("LogicalName", EntityNameRef)]
        [DataRow(BehindLookupAttribute, BehindLookupValue)]
        public void ToStringFormat_Test(string format, string expected)
        {
            //Arrange 
            var entref = Target.GetAttributeValue<EntityReference>(LookupAttribute);
            var Printer = new EntityReferencePrintable(entref, Context, new AttributeConverterService(Context));

            //Act
            var result = Printer.ToString(format);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToStringFormat_IdTest()
        {
            //Arrange 
            var entref = Target.GetAttributeValue<EntityReference>(LookupAttribute);
            var Printer = new EntityReferencePrintable(entref, Context, new AttributeConverterService(Context));

            //Act
            var result = Printer.ToString("Id");

            //Assert
            Assert.AreEqual(entref.Id.ToString(), result);
        }

        [TestMethod]
        public void ToStringFormat_MissingAttributeTest()
        {
            //Arrange 
            var entref = Target.GetAttributeValue<EntityReference>(LookupAttribute);
            var Printer = new EntityReferencePrintable(entref, Context, new AttributeConverterService(Context));

            //Act
            var result = Printer.ToString("NotPresent");

            //Assert
            Assert.IsNull(result);
        }
    }
}