using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Services;
using System;

namespace mwo.D365NameCombiner.Plugins.Tests
{
    [TestClass()]
    public class CombinerServiceTests
    {
        private const string EntityName = "account";
        private const string EntityNameRef = "contact";
        private const string StringAttribute = "name";
        private const string StringValue = "abc";
        private const string IntAttribute = "accountnumber";
        private const int IntValue = 123;
        private const string BooleanAttribute = "donotemail";
        private const bool BooleanValue = true;
        private const string EnumAttribute = "accounttype";
        private const int ValueOne = 1;
        private const int ValueTwo = 2;
        private OptionSetValue EnumValue = new OptionSetValue(ValueOne);
        private const string EnumsAttribute = "accountgroups";
        private OptionSetValueCollection EnumsValue = new OptionSetValueCollection
        {
            new OptionSetValue(ValueOne),
            new OptionSetValue(ValueTwo)
        };
        private const string DecimalAttribute = "decimal";
        private const decimal DecimalValue = 1.24M;
        private const string DoubleAttribute = "double";
        private const double DoubleValue = 2.48;
        private const string LookupAttribute = "maincontactid";
        private EntityReference LookupValue;
        private const string BehindLookupAttribute = "fullname";
        private const string BehindLookupValue = "Test, Test";
        private const string MoneyAttribute = "estimatedvalue";
        private Money MoneyValue = new Money(3.72M);
        private const string GuidAttribute = "accountid";
        private Guid GuidValue = Guid.NewGuid();
        private const string NullAttribute = "null";
        private Entity Target;
        private CombinerService Service;
        private const string SimpleFormat = "{0}";


        [TestInitialize]
        public void Initialize()
        {
            var svc = new XrmFakedContext().GetOrganizationService();

            var contact = new Entity(EntityNameRef)
            {
                [BehindLookupAttribute] = BehindLookupValue
            };
            contact.Id = svc.Create(contact);
            LookupValue = contact.ToEntityReference();

            Target = new Entity(EntityName)
            {
                [StringAttribute] = StringValue,
                [IntAttribute] = IntValue,
                [EnumAttribute] = EnumValue,
                [EnumsAttribute] = EnumsValue,
                [BooleanAttribute] = BooleanValue,
                [DecimalAttribute] = DecimalValue,
                [DoubleAttribute] = DoubleValue,
                [LookupAttribute] = LookupValue,
                [MoneyAttribute] = MoneyValue,
                [GuidAttribute] = GuidValue,
                [NullAttribute] = null
            };

            Service = new CombinerService(Target);
        }


        [DataTestMethod]
        [DataRow(StringAttribute, StringValue)]
        [DataRow(IntAttribute, IntValue)]
        [DataRow(EnumAttribute, ValueOne)]
        [DataRow(EnumsAttribute, "1 2")]
        public void Combine_SimpleFormatTest(string attr, object expected)
        {
            //Act
            var result = Service.Combine(SimpleFormat, attr);

            //Assert
            Assert.AreEqual(expected.ToString(), result);
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