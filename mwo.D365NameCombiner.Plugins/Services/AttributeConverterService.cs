using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Decorators;
using mwo.D365NameCombiner.Plugins.Helpers;
using mwo.D365NameCombiner.Plugins.Models;
using System;

namespace mwo.D365NameCombiner.Plugins.Services
{
    public class AttributeConverterService
    {
        private ICRMContext Context;

        public AttributeConverterService(ICRMContext context)
        {
            Context = context;
        }

        public object Convert(Entity ent, string attribute)
        {
            Context.Trace.Trace($"Converting Attribute {attribute} on {ent?.LogicalName}");
            switch (EntityHelper.GetValue(ent, attribute))
            {
                case string s:
                    return s;
                case int i:
                    return i;
                case bool b:
                    return b;
                case double d:
                    return d;
                case decimal dc:
                    return dc;
                case Guid g:
                    return g;
                case DateTime dt:
                    return dt;
                case EntityReference e:
                    return new EntityReferencePrintable(e, Context, this);
                case OptionSetValue o:
                    return new OptionSetValuePrintable(o, Context, ent.LogicalName, attribute);
                case OptionSetValueCollection os:
                    return new OptionSetValueCollectionPrintable(os, Context, ent.LogicalName, attribute);
                case Money m:
                    return new MoneyPrintable(m);
                default:
                    return null;
            }
        }
    }
}
