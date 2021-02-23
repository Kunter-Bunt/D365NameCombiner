using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Executables;
using mwo.D365NameCombiner.Plugins.Models;
using mwo.D365NameCombiner.Plugins.Services;
using System;

namespace mwo.D365NameCombiner.Plugins.Entrypoints
{
    public class PreOpNameCombination : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = new CRMPluginContext(serviceProvider);

            var attributeService = new AttributeConverterService(context);
            var expressionService = new ExpressionConverterService(context);
            var combinerService = new CombinerService(context.Subject, attributeService, expressionService, context);

            var executable = new OwnNameCombinationExecutable(context.Trace, combinerService);
            executable.Execute(
                context.Subject.ToEntity<mwo_NameCombination>(),
                context.Target.ToEntity<mwo_NameCombination>());
        }
    }
}
