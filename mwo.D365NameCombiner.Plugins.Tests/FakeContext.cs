using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Models;

namespace mwo.D365NameCombiner.Plugins.Tests
{
    class FakeContext : ICRMContext
    {
        public Entity Target { get; }
        public Entity PreImage { get; }
        public Entity Subject { get; }
        public IOrganizationService OrgService { get; }
        public IPluginExecutionContext PluginContext { get; }
        public ITracingService Trace { get; }

        public FakeContext(XrmFakedContext ctx, Entity target, Entity preImage, Entity postImage)
        {
            Trace = ctx.GetFakeTracingService();
            OrgService = ctx.GetOrganizationService();
            PluginContext = ctx.GetDefaultPluginContext();
            Target = target;
            PreImage = preImage;
            Subject = postImage;
        }
    }
}
