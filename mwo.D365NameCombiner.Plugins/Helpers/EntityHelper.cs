using Microsoft.Xrm.Sdk;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace mwo.D365NameCombiner.Plugins.Helpers
{
    [DynamicLinqType]
    public static class EntityHelper
    {
        public static object GetValue(Entity ent, string attr)
        {
            return ent.Contains(attr) ? ent[attr] : null;
        }

        public static bool HasValue(Entity ent, string attr)
        {
            var value = GetValue(ent, attr);
            if (value is string stringValue)
                return !string.IsNullOrEmpty(stringValue);
            else
                return value != null;
        }
    }
}
