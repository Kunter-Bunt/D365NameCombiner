using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using mwo.D365NameCombiner.Plugins.Extensions;
using mwo.D365NameCombiner.Plugins.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mwo.D365NameCombiner.Plugins.Decorators
{
    public class OptionSetValuePrintable : IFormattable
    {
        public OptionSetValue Option { get; }
        public ICRMContext Context { get; }
        public string EntityName { get; }
        public string FieldName { get; }

        public OptionSetValuePrintable(OptionSetValue option, ICRMContext context, string entityName, string fieldName)
        {
            Option = option;
            Context = context;
            EntityName = entityName;
            FieldName = fieldName;
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
            if (dict.ContainsKey("LCID") && int.TryParse(dict["LCID"], out int lcid))
                return ResolveByLCID(lcid);
            else if (dict.Any())
                return ResolveNameForValue(dict);

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

        private string ResolveByLCID(int lcid)
        {
            try
            {
                var attributeResponse = (RetrieveAttributeResponse)Context.OrgService.Execute(new RetrieveAttributeRequest
                {
                    EntityLogicalName = EntityName,
                    LogicalName = FieldName
                });
                var meta = (EnumAttributeMetadata)attributeResponse.AttributeMetadata;
                var option = meta.OptionSet.Options.First(_ => _.Value == Option.Value);
                return option.Label.LocalizedLabels.Where(_ => _.LanguageCode == lcid).FirstOrDefault()?.Label
                        ?? option.Label.UserLocalizedLabel?.Label;
            }
            catch (Exception ex)
            {
                Context.Trace.Trace($"Exception while trying to resolve Optionset Label: {ex}\n{ex.Message}");
                return null;
            }
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }
    }
}
