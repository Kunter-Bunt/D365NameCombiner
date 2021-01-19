using Microsoft.VisualStudio.TestTools.UnitTesting;
using mwo.D365NameCombiner.Plugins.Services;

namespace mwo.D365NameCombiner.Plugins.Tests
{
    [TestClass]
    public class CombinerServiceTests : TestBase
    {
        protected CombinerService Service;

        [TestInitialize]
        public void Initialize()
        {
            Service = new CombinerService(Target, new AttributeConverterService(Context), new ExpressionConverterService(Context), Context);
        }

        [DataTestMethod]
        [DataRow(StringAttribute, StringValue)]
        [DataRow(IntAttribute, IntValue)]
        [DataRow(EnumAttribute, ValueOne)]
        [DataRow(EnumsAttribute, "1 2")]
        [DataRow(BooleanAttribute, "True")]
        [DataRow(DecimalAttribute, "1.24")]
        [DataRow(DoubleAttribute, "2.48")]
        [DataRow(MoneyAttribute, "3.72")]
        [DataRow(NullAttribute, "")]
        [DataRow(SimpleExpression, SimpleExpressionExpected)]
        public void Combine_SimpleFormatTest(string attr, object expected)
        {
            //Act
            var result = Service.Combine(SimpleFormat, attr);

            //Assert
            Assert.AreEqual(expected.ToString(), result, $"Attribute: {attr}");
        }

        [TestMethod]
        public void Combine_GuidTest()
        {
            //Act
            var result = Service.Combine(SimpleFormat, GuidAttribute);

            //Assert
            Assert.AreEqual(GuidValue.ToString(), result);
        }

        [TestMethod]
        public void Combine_ReferenceTest()
        {
            //Act
            var result = Service.Combine(SimpleFormat, LookupAttribute);

            //Assert
            StringAssert.Contains(result, LookupValue.LogicalName);
            StringAssert.Contains(result, LookupValue.Id.ToString());
        }

        [TestMethod]
        public void Combine_ReferenceNameTest()
        {
            //Act
            var result = Service.Combine("{0:Name}", LookupAttribute);

            //Assert
            Assert.AreEqual(LookupValueName, result);
        }

        [TestMethod]
        public void Combine_MoneyFormaatTest()
        {
            //Act
            var result = Service.Combine("{0:C3}", MoneyAttribute);

            //Assert
            Assert.AreEqual(string.Format(MoneyDecimalValue.ToString("C3")), result);
        }

        [TestMethod]
        public void Combine_EnumHelloTest()
        {
            //Act
            var result = Service.Combine("{0:1=Hello}", EnumAttribute);

            //Assert
            Assert.AreEqual("Hello", result);
        }

        [TestMethod]
        public void Combine_EnumsHelloTest()
        {
            //Arrange
            var expected = "Hello 2";

            //Act
            var result = Service.Combine("{0:1=Hello}", EnumsAttribute);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}