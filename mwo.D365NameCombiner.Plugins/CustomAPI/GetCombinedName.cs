using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using mwo.D365NameCombiner.Plugins.Executables;
using mwo.D365NameCombiner.Plugins.Models;
using mwo.D365NameCombiner.Plugins.Services;
using System;

namespace mwo.D365NameCombiner.Plugins.CustomAPI
{
    [CrmPluginRegistration("mwo_GetCombinedName")]
    public class GetCombinedName : IPlugin
    {
        public const string RequestParameter_TargetId = "TargetId"; 
        public const string RequestParameter_TargetLogicalName = "TargetLogicalName";
        public const string RequestParameter_Format = "Format";
        public const string ResponseParameter_PO = "P0";
        public const string ResponseParameter_P1 = "P1";
        public const string ResponseParameter_P2 = "P2";
        public const string ResponseParameter_P3 = "P3";
        public const string ResponseParameter_P4 = "P4";
        public const string ResponseParameter_P5 = "P5";
        public const string ResponseParameter_P6 = "P6";
        public const string ResponseParameter_P7 = "P7";
        public const string ResponseParameter_P8 = "P8";
        public const string ResponseParameter_P9 = "P9";
        public const string ResponseParameter_CombinedName = "CombinedName";
        public const string ResponseParameter_HasError = "HasError";
        public const string ResponseParameter_ErrorMessage = "ErrorMessage";

        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                var context = new CRMPluginContext(serviceProvider);

                context.PluginContext.InputParameters.TryGetValue(RequestParameter_TargetId, out var targetIdString);
                context.PluginContext.InputParameters.TryGetValue(RequestParameter_TargetLogicalName, out var targetLogicalName);
                context.PluginContext.InputParameters.TryGetValue(RequestParameter_Format, out var format);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_PO, out var p0);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_P1, out var p1);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_P2, out var p2);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_P3, out var p3);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_P4, out var p4);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_P5, out var p5);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_P6, out var p6);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_P7, out var p7);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_P8, out var p8);
                context.PluginContext.InputParameters.TryGetValue(ResponseParameter_P9, out var p9);

                if (!Guid.TryParse(targetIdString as string, out var targetId))
                {
                    return;
                }

                var target = context.OrgService.Retrieve(targetLogicalName as string, targetId, new ColumnSet(true));

                var attributeService = new AttributeConverterService(context);
                var combinerService = new CombinerService(target, attributeService, context);

                var executable = new NameCombinationExecutable(combinerService, context);
                var combinedName = executable.Execute(format as string,
                    p0 as string,
                    p1 as string,
                    p2 as string,
                    p3 as string,
                    p4 as string,
                    p5 as string,
                    p6 as string,
                    p7 as string,
                    p8 as string,
                    p9 as string);

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
