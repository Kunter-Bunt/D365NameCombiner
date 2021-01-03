using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
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
        private OptionSetValue EnumValue = new OptionSetValue(1);
        private const string EnumsAttribute = "accountgroups";
        private OptionSetValueCollection EnumsValue = new OptionSetValueCollection
        {
            new OptionSetValue(1),
            new OptionSetValue(2)
        };
        private const string DecimalAttribute = "decimal";
        private const Decimal DecimalValue = 1.24M;
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

        [TestMethod]
        public void Combine_StringTest()
        {
            //Act
            var result = Service.Combine(SimpleFormat, StringAttribute);

            //Assert
            Assert.AreEqual(StringValue, result);
        }

        [TestMethod]
        public void Combine_IntTest()
        {
            //Act
            var result = Service.Combine(SimpleFormat, IntAttribute);

            //Assert
            Assert.AreEqual($"{IntValue}", result);
        }
    }
}