using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            switch(ent[attribute])
            {
                case string s: 
                    return s;
                case int i:
                    return i;
                case OptionSetValue o:
                    return new OptionSetValuePrintable(o);
                case OptionSetValueCollection os:
                    return new OptionSetValueCollectionPrintable(os);
                default:
                    return null;
            }
        }
    }
}
