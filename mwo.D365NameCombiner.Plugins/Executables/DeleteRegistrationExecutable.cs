using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Models;

namespace mwo.D365NameCombiner.Plugins.Executables
{
    public class DeleteRegistrationExecutable
    {
        private ICRMContext Context;
        private ITracingService Trace;

        public DeleteRegistrationExecutable(ICRMContext context)
        {
            Context = context;
            Trace = context.Trace;
        }

        public void Execute(mwo_NameCombination subject)
        {
            Trace.Trace(nameof(DeleteRegistrationExecutable));

            Trace.Trace(nameof(subject.mwo_CreateStep));
            if (subject.mwo_CreateStep != null)
                DeleteStep(subject.mwo_CreateStep);

            Trace.Trace(nameof(subject.mwo_UpdateStep));
            if (subject.mwo_UpdateStep != null)
                DeleteStep(subject.mwo_UpdateStep);
        }

        private void DeleteStep(EntityReference step)
        {
            Context.OrgService.Delete(step.LogicalName, step.Id);
        }
    }
}
