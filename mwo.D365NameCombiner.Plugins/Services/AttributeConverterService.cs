using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Decorators;
using System;

namespace mwo.D365NameCombiner.Plugins.Services
{
    public class AttributeConverterService
    {
        public AttributeConverterService()
        {

        }

        public object Convert(Entity ent, string attribute)
        {
            if (!ent.Contains(attribute)) return null;

            switch (ent[attribute])
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
                case EntityReference e:
                    return new EntityReferencePrintable(e);
                case OptionSetValue o:
                    return new OptionSetValuePrintable(o);
                case OptionSetValueCollection os:
                    return new OptionSetValueCollectionPrintable(os);
                case Money m:
                    return new MoneyPrintable(m);
                default:
                    return null;
            }
        }
    }
}
