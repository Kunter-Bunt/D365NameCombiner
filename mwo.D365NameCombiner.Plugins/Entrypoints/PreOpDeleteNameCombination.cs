using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Executables;
using mwo.D365NameCombiner.Plugins.Models;
using System;

namespace mwo.D365NameCombiner.Plugins.Entrypoints
{
    public class PreOpDeleteNameCombination : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = new CRMPluginContext(serviceProvider);

            var executable = new DeleteRegistrationExecutable(context);
            executable.Execute(context.Subject.ToEntity<mwo_NameCombination>());
        }
    }
}
