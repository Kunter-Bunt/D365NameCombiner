using Microsoft.Xrm.Sdk;

namespace mwo.D365NameCombiner.Plugins.Models
{
    /// <summary>
    /// Combination Interface that can be generated from a IServiceProvider.
    /// </summary>
    public interface ICRMContext
    {
        Entity Target { get; }
        Entity PreImage { get; }
        Entity PostImage { get; }
        IOrganizationService OrgService { get; }
        IPluginExecutionContext PluginContext { get; }
        ITracingService Trace { get; }
    }
}
