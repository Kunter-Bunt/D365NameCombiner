using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using mwo.D365NameCombiner.Plugins.Executables;
using mwo.D365NameCombiner.Plugins.Models;
using mwo.D365NameCombiner.Plugins.Services;
using System;

namespace mwo.D365NameCombiner.Plugins.CustomAPI
{
    [CrmPluginRegistration("mwo_GetCombinedNameByConfig")]
    public class GetCombinedNameByConfig : IPlugin
    {
        public const string RequestParameter_TargetId = "TargetId";
        public const string RequestParameter_TargetLogicalName = "TargetLogicalName";
        public const string RequestParameter_ConfigId = "ConfigId";
        public const string ResponseParameter_CombinedName = "CombinedName";
        public const string ResponseParameter_HasError = "HasError";
        public const string ResponseParameter_ErrorMessage = "ErrorMessage";

        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                var context = new CRMPluginContext(serviceProvider);

                context.PluginContext.InputParameters.TryGetValue(RequestParameter_ConfigId, out var configId);
                context.PluginContext.InputParameters.TryGetValue(RequestParameter_TargetId, out var targetIdString);
                context.PluginContext.InputParameters.TryGetValue(RequestParameter_TargetLogicalName, out var targetLogicalName);
                if (!Guid.TryParse(targetIdString as string, out var targetId))
                {
                    return;
                }

                var target = context.OrgService.Retrieve(targetLogicalName as string, targetId, new ColumnSet(true));

                var attributeService = new AttributeConverterService(context);
                var expressionService = new ExpressionConverterService(context);
                var combinerService = new CombinerService(target, attributeService, expressionService, context);

                var executable = new NameCombinationExecutable(combinerService, context);
                var combinedName = executable.Execute(configId as string);

                context.PluginContext.OutputParameters.AddOrUpdateIfNotNull(ResponseParameter_CombinedName, combinedName);

            }
            catch (Exception ex)
            {
                var errorMessage = $"ERROR: {ex.Message}";
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                trace.Trace(errorMessage);
                var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                pluginContext.OutputParameters.AddOrUpdateIfNotNull(ResponseParameter_HasError, true);
                pluginContext.OutputParameters.AddOrUpdateIfNotNull(ResponseParameter_ErrorMessage, errorMessage);
            }
        }
    }
}
