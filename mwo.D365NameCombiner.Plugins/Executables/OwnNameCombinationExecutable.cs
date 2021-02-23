using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Models;

namespace mwo.D365NameCombiner.Plugins.Executables
{
    public class OwnNameCombinationExecutable
    {
        private ITracingService Trace;

        public OwnNameCombinationExecutable(ITracingService trace)
        {
            Trace = trace;
        }

        public void Execute(mwo_NameCombination subject, mwo_NameCombination target)
        {
            Trace.Trace(nameof(OwnNameCombinationExecutable));
            target.mwo_Name = $"{subject.mwo_Table} - {subject.mwo_Column}";
            Trace.Trace("Done");
        }
    }
}
