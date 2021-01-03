using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mwo.D365NameCombiner.Plugins.Decorators
{
    public class OptionSetValuePrintable : IFormattable
    {
        public OptionSetValue Option { get; }

        public OptionSetValuePrintable(OptionSetValue option)
        {
            Option = option;
        }

        public override string ToString()
        {
            return Option?.Value.ToString();
        }

        public string ToString(string format)
        {
            if (Option == null) return null;

            if (string.IsNullOrEmpty(format)) return ToString();

            var dict = format.ToDictionary();
            if (dict.Any()) return ResolveNameForValue(dict);

            return ToString();
        }

        private string ResolveNameForValue(Dictionary<string, string> dict)
        {
            var stringValue = ToString();

            if (dict.ContainsKey(stringValue))
                return dict[stringValue];
            else
                return stringValue;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }
    }
}
