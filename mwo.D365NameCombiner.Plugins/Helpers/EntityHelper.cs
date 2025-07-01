using Microsoft.Xrm.Sdk;
namespace mwo.D365NameCombiner.Plugins.Helpers
{
    public static class EntityHelper
    {
        public static object GetValue(Entity ent, string attr)
        {
            return ent.Contains(attr) ? ent[attr] : null;
        }
    }
}
