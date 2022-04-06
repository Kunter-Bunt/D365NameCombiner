using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Executables;
using mwo.D365NameCombiner.Plugins.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace mwo.D365NameCombiner.Plugins.Plugins
{
    [ExcludeFromCodeCoverage]
    [CrmPluginRegistration(MessageNameEnum.Delete, mwo_NameCombination.EntityLogicalName, StageEnum.PreOperation, ExecutionModeEnum.Synchronous, FilteringAttributes, DeleteName, Rank, IsolationModeEnum.Sandbox, Image1Type = ImageTypeEnum.PreImage, Image1Name = CRMPluginContext.PreImageName, Image1Attributes = "")]
    public class PreOpDeleteNameCombination : IPlugin
    {
        public const int Rank = 1;
        public const string DeleteName = nameof(PreOpDeleteNameCombination) + nameof(MessageNameEnum.Delete);
        public const string FilteringAttributes = "";

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = new CRMPluginContext(serviceProvider);

            var executable = new DeleteRegistrationExecutable(context);
            executable.Execute(context.Subject.ToEntity<mwo_NameCombination>());
        }
    }
}
