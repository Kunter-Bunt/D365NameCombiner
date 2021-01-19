using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Executables;
using mwo.D365NameCombiner.Plugins.Models;
using mwo.D365NameCombiner.Plugins.Services;
using System;

namespace mwo.D365NameCombiner.Plugins.EntryPoints
{
    public class NameCombiner : IPlugin
    {
        private string UnsecureConfiguration { get; set; }
        private string SecureConfiguration { get; set; }

        public NameCombiner(string unsecureConfiguration, string secureConfiguration)
        {
            UnsecureConfiguration = unsecureConfiguration;
            SecureConfiguration = secureConfiguration;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = new CRMPluginContext(serviceProvider);

            var attributeService = new AttributeConverterService(context);
            var expressionService = new ExpressionConverterService(context);
            var combinerService = new CombinerService(context.Subject, attributeService, expressionService, context);

            var executable = new NameCombinationExecutable(combinerService, context);
            executable.Execute(context.Target, UnsecureConfiguration);
        }
    }
}
