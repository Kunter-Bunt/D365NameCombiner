using FakeXrmEasy;
using FakeXrmEasy.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using mwo.D365NameCombiner.Plugins.Models;
using System;

namespace mwo.D365NameCombiner.Plugins.Tests
{
    public abstract class TestBase
    {
        protected const string EntityName = "account";
        protected const string EntityNameRef = mwo_NameCombination.EntityLogicalName;

        protected const string StringAttribute = nameof(StringAttribute);
        protected const string StringValue = nameof(StringValue);

        protected const string IntAttribute = nameof(IntAttribute);
        protected const int IntValue = 123;

        protected const string BooleanAttribute = nameof(BooleanAttribute);
        protected const bool BooleanValue = true;

        protected const int ValueOne = 1;
        protected const string ValueOneName = "Hello";
        protected const int ValueTwo = 2;
        protected const string ValueTwoName = "World";

        protected const int LCIDEnglish = 1033;

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

        protected const string BehindLookupAttribute = mwo_NameCombination.Fields.mwo_Table;
        protected const string BehindLookupValue = nameof(BehindLookupValue);

        protected const string MoneyAttribute = nameof(MoneyAttribute);
        protected const decimal MoneyDecimalValue = 3.72M;
        protected readonly Money MoneyValue = new Money(MoneyDecimalValue);

        protected const string GuidAttribute = nameof(GuidAttribute);
        protected Guid GuidValue = Guid.NewGuid();

        protected const string DateTimeAttribute = nameof(DateTimeAttribute);
        protected DateTime DateTimeValue = new DateTime(2025, 07, 01, 12, 23, 30);

        protected const string NullAttribute = nameof(NullAttribute);

        protected const string CombinedAttribute = nameof(CombinedAttribute);

        protected mwo_NameCombination Config;

        protected Entity Target;

        protected const string SimpleFormat = "{0}";

        protected const string SimpleExpression = @"_ => EntityHelper.GetValue(_.Subject, ""StringAttribute"")";
        protected const string SimpleExpressionExpected = StringValue;

        protected IOrganizationService OrgService;
        protected ICRMContext Context;
        protected XrmFakedContext FakeEasyContext;

        [TestInitialize]
        public void Init()
        {
            FakeEasyContext = new XrmFakedContext();
            OrgService = FakeEasyContext.GetOrganizationService();

            SetupMetadata();
            SetupTarget();
            SetupConfig();

            Context = new FakeContext(FakeEasyContext, Target, null, Target);
        }

        private void SetupMetadata()
        {
            var entityMeta = new EntityMetadata { LogicalName = EntityName };

            var optionMeta = new OptionSetMetadata
            {
                OptionSetType = OptionSetType.Picklist,
                Options =
                {
                    new OptionMetadata(new Label(ValueOneName, LCIDEnglish), ValueOne),
                    new OptionMetadata(new Label(ValueTwoName, LCIDEnglish), ValueTwo),
                }
            };

            var enumMeta = new PicklistAttributeMetadata
            {
                LogicalName = EnumAttribute,
                OptionSet = optionMeta
            };
            var enumsMeta = new MultiSelectPicklistAttributeMetadata
            {
                LogicalName = EnumsAttribute,
                OptionSet = optionMeta
            };

            entityMeta.SetAttribute(enumMeta);
            entityMeta.SetAttribute(enumsMeta);
            FakeEasyContext.SetEntityMetadata(entityMeta);
        }

        private void SetupTarget()
        {
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
                [DateTimeAttribute] = DateTimeValue,
                [NullAttribute] = null
            };
        }

        private void SetupConfig()
        {
            Config = new mwo_NameCombination()
            {
                mwo_Table = EntityName,
                mwo_Column = CombinedAttribute,
                mwo_Format = SimpleFormat,
                mwo_format0 = StringAttribute
            };
            Config.Id = OrgService.Create(Config);
        }
    }
}
