using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using mwo.D365NameCombiner.Plugins.Models;
using mwo.D365NameCombiner.Plugins.Services;
using System;

namespace mwo.D365NameCombiner.Plugins.Decorators
{
    public class EntityReferencePrintable : IFormattable
    {
        public EntityReference Reference { get; }
        public ICRMContext Context { get; }
        public AttributeConverterService AttributeService { get; }

        public EntityReferencePrintable(EntityReference reference, ICRMContext context, AttributeConverterService attributeService)
        {
            Reference = reference;
            Context = context;
            AttributeService = attributeService;
        }

        public override string ToString()
        {
            if (Reference == null) return null;
            return $"{Reference.LogicalName}({Reference.Id})";
        }

        public string ToString(string format)
        {
            if (Reference == null) return null;

            if (string.IsNullOrEmpty(format)) return ToString();

            if (format.ToLower() == "name")
                return Reference.Name;
            else if (format.ToLower() == "logicalname")
                return Reference.LogicalName;
            else if (format.ToLower() == "id")
                return Reference.Id.ToString();
            else
                return ResolveField(format);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }

        private string ResolveField(string format)
        {
            try
            {
                var parts = format.Split(new[] { ';' }, 2);
                var attrName = parts[0];
                var futherFormats = parts.Length > 1 ? parts[1] : null;

                var ent = Context.OrgService.Retrieve(Reference.LogicalName, Reference.Id, new ColumnSet(attrName));
                var attr = AttributeService.Convert(ent, attrName);

                if (futherFormats != null && attr is IFormattable formatAttr)
                    return formatAttr.ToString(futherFormats, null);

                return attr.ToString();
            }
            catch (Exception ex)
            {
                Context.Trace.Trace($"Exception while trying to resolve Entity Reference: {ex}\n{ex.Message}");
                return null;
            }
        }
    }
}
