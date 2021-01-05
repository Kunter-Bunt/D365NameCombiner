using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Models;
using System;

namespace mwo.D365NameCombiner.Plugins.Tests
{
    public abstract class TestBase
    {
        protected const string EntityName = "account";
        protected const string EntityNameRef = "contact";

        protected const string StringAttribute = nameof(StringAttribute);
        protected const string StringValue = nameof(StringAttribute);

        protected const string IntAttribute = nameof(IntAttribute);
        protected const int IntValue = 123;

        protected const string BooleanAttribute = nameof(BooleanAttribute);
        protected const bool BooleanValue = true;

        protected const int ValueOne = 1;
        protected const int ValueTwo = 2;

        protected const string EnumAttribute = nameof(EnumAttribute);
        protected readonly OptionSetValue EnumValue = new OptionSetValue(ValueOne);

        protected const string EnumsAttribute = nameof(EnumsAttribute);
        protected readonly OptionSetValueCollection EnumsValue = new OptionSetValueCollection
        {
            new OptionSetValue(ValueOne),
            new OptionSetValue(ValueTwo)
        };

        protected const string DecimalAttribute = nameof(DecimalAttribute);
        protected const decimal DecimalValue = 1.24M;

        protected const string DoubleAttribute = nameof(DoubleAttribute);
        protected const double DoubleValue = 2.48;

        protected const string LookupAttribute = nameof(LookupAttribute);
        protected const string LookupValueName = nameof(LookupValueName);
        protected EntityReference LookupValue;

        protected const string BehindLookupAttribute = nameof(BehindLookupAttribute);
        protected const string BehindLookupValue = nameof(BehindLookupValue);

        protected const string MoneyAttribute = nameof(MoneyAttribute);
        protected const decimal MoneyDecimalValue = 3.72M;
        protected readonly Money MoneyValue = new Money(MoneyDecimalValue);

        protected const string GuidAttribute = nameof(GuidAttribute);
        protected Guid GuidValue = Guid.NewGuid();

        protected const string NullAttribute = nameof(NullAttribute);

        protected Entity Target;
        protected const string SimpleFormat = "{0}";

        protected IOrganizationService OrgService;
        protected ICRMContext Context;
        protected XrmFakedContext FakeEasyContext;

        [TestInitialize]
        public void Init()
        {
            FakeEasyContext = new XrmFakedContext();
            OrgService = FakeEasyContext.GetOrganizationService();

            var contact = new Entity(EntityNameRef)
            {
                [BehindLookupAttribute] = BehindLookupValue
            };
            contact.Id = OrgService.Create(contact);
            var reference = contact.ToEntityReference();
            reference.Name = LookupValueName;
            LookupValue = reference;

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

            Context = new FakeContext(FakeEasyContext, Target, null, Target);
        }
    }
}
