using Microsoft.VisualStudio.TestTools.UnitTesting;
using mwo.D365NameCombiner.Plugins.Decorators;
using mwo.D365NameCombiner.Plugins.Tests;
using System;

namespace mwo.D365NameCombiner.Plugins.Services.Tests
{
    [TestClass]
    public class AttributeConverterServiceTests : TestBase
    {
        [DataTestMethod]
        [DataRow(StringAttribute, typeof(string))]
        [DataRow(IntAttribute, typeof(int))]
        [DataRow(DecimalAttribute, typeof(decimal))]
        [DataRow(DoubleAttribute, typeof(double))]
        [DataRow(BooleanAttribute, typeof(bool))]
        [DataRow(GuidAttribute, typeof(Guid))]
        [DataRow(EnumAttribute, typeof(OptionSetValuePrintable))]
        [DataRow(EnumsAttribute, typeof(OptionSetValueCollectionPrintable))]
        [DataRow(LookupAttribute, typeof(EntityReferencePrintable))]
        [DataRow(MoneyAttribute, typeof(MoneyPrintable))]
        public void Convert_Test(string attr, Type outT)
        {
            //Arrange
            var service = new AttributeConverterService(Context);

            //Act
            var result = service.Convert(Target, attr);

            //Assert
            Assert.AreEqual(outT, result?.GetType());
        }

        [TestMethod]
        public void Convert_NullTest()
        {
            //Arrange
            var service = new AttributeConverterService(Context);

            //Act
            var result = service.Convert(Target, NullAttribute);

            //Assert
            Assert.IsNull(result);
        }
    }
}