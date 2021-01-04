using Microsoft.Xrm.Sdk;
using System;

namespace mwo.D365NameCombiner.Plugins.Decorators
{
    public class EntityReferencePrintable : IFormattable
    {
        public EntityReference Reference { get; }

        public EntityReferencePrintable(EntityReference reference)
        {
            Reference = reference;
        }

        public override string ToString()
        {
            return $"{Reference.LogicalName}({Reference.Id})";
        }

        public string ToString(string format)
        {
            if (Reference == null) return null;

            if (string.IsNullOrEmpty(format)) return ToString();

            if (format.ToLower() == "name")
            {
                return Reference.Name;
            }

            return ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }
    }
}
