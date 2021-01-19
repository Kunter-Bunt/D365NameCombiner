using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Extensions;
using System;

namespace mwo.D365NameCombiner.Plugins.Models
{
    public class CRMPluginContext : ICRMContext
    {
        public const string TargetName = "Target";
        public const string PreImageName = "Default";

        public Entity Target { get; private set; }
        public Entity PreImage { get; private set; }
        public Entity Subject { get; private set; }
        public IOrganizationService OrgService { get; private set; }
        public IPluginExecutionContext PluginContext { get; private set; }
        public IOrganizationServiceFactory Factory { get; private set; }
        public ITracingService Trace { get; private set; }

        public CRMPluginContext(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new InvalidPluginExecutionException(nameof(serviceProvider));
            PluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            Factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            Trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (PluginContext.InputParameters.ContainsKey(TargetName)
                && (PluginContext.InputParameters[TargetName] is Entity targetEntity))
                Target = targetEntity;
            else if (PluginContext.InputParameters.ContainsKey(TargetName)
                && (PluginContext.InputParameters[TargetName] is EntityReference targetRef))
                Target = new Entity(targetRef.LogicalName, targetRef.Id);
            else
            {
                Trace.Trace("Context did not have an Entity as Target, aborting.");
                throw new InvalidPluginExecutionException(TargetName);
            }

            if (PluginContext.PreEntityImages.ContainsKey(PreImageName)
                && (PluginContext.PreEntityImages[PreImageName] is Entity preImageEntity))
                PreImage = preImageEntity;
            Trace.Trace($"PreImage present: {PreImage != null}");

            Subject = PreImage == null ? Target : PreImage.Merge(Target);

            OrgService = Factory.CreateOrganizationService(PluginContext.UserId);
        }
    }
}
