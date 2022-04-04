using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Executables;
using mwo.D365NameCombiner.Plugins.Models;
using mwo.D365NameCombiner.Plugins.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace mwo.D365NameCombiner.Plugins.Plugins
{
    [ExcludeFromCodeCoverage]
    [CrmPluginRegistration(MessageNameEnum.Create, mwo_NameCombination.EntityLogicalName, StageEnum.PostOperation, ExecutionModeEnum.Asynchronous, FilteringAttributes, CreateName, Rank, IsolationModeEnum.Sandbox)]
    public class NameCombiner : IPlugin
    {
        public const int Rank = 1;
        public const string CreateName = nameof(NameCombiner) + nameof(MessageNameEnum.Create) + "_DummyRegistration";
        public const string FilteringAttributes = "";

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
