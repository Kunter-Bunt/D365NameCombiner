using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Extensions;
using mwo.D365NameCombiner.Plugins.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mwo.D365NameCombiner.Plugins.Decorators
{
    public class OptionSetValueCollectionPrintable : IFormattable
    {
        public OptionSetValueCollection Options { get; }
        public ICRMContext Context { get; }
        public string EntityName { get; }
        public string FieldName { get; }

        public OptionSetValueCollectionPrintable(OptionSetValueCollection options, ICRMContext context, string entityName, string fieldName)
        {
            Options = options;
            Context = context;
            EntityName = entityName;
            FieldName = fieldName;
        }

        public override string ToString()
        {
            return ToString("Separator= ");
        }

        public string ToString(string format)
        {
            if (Options == null || !Options.Any()) return null;
            format = format ?? "";

            var dict = format.ToDictionary();
            var joiner = dict.ContainsKey("Separator") ? dict["Separator"] : " ";

            var formattedOptions = new List<string>();
            foreach (var option in Options)
                formattedOptions.Add(new OptionSetValuePrintable(option, Context, EntityName, FieldName).ToString(format));

            return string.Join(joiner, formattedOptions.ToArray()); ;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }
    }
}
