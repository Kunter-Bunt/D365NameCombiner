using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Executables;
using mwo.D365NameCombiner.Plugins.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace mwo.D365NameCombiner.Plugins.Plugins
{
    [ExcludeFromCodeCoverage]
    [CrmPluginRegistration(MessageNameEnum.Create, mwo_NameCombination.EntityLogicalName, StageEnum.PostOperation, ExecutionModeEnum.Synchronous, FilteringAttributes, CreateName, Rank, IsolationModeEnum.Sandbox)]
    [CrmPluginRegistration(MessageNameEnum.Update, mwo_NameCombination.EntityLogicalName, StageEnum.PostOperation, ExecutionModeEnum.Synchronous, FilteringAttributes, UpdateName, Rank, IsolationModeEnum.Sandbox, Image1Type = ImageTypeEnum.PreImage, Image1Name = CRMPluginContext.PreImageName, Image1Attributes = "")]
    public class PostOpNameCombination : IPlugin
    {
        public const int Rank = 1;
        public const string CreateName = nameof(PostOpNameCombination) + nameof(MessageNameEnum.Create);
        public const string UpdateName = nameof(PostOpNameCombination) + nameof(MessageNameEnum.Update);
        public const string FilteringAttributes = "";

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = new CRMPluginContext(serviceProvider);

            var executable = new RegistrationExecutable(context);
            executable.Execute(context.Subject.ToEntity<mwo_NameCombination>());
        }
    }
}
