using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Models;
using mwo.D365NameCombiner.Plugins.Services;

namespace mwo.D365NameCombiner.Plugins.Executables
{
    public class OwnNameCombinationExecutable
    {
        private ITracingService Trace;
        private CombinerService Combiner;

        public OwnNameCombinationExecutable(ITracingService trace, CombinerService combiner)
        {
            Trace = trace;
            Combiner = combiner;
        }

        public void Execute(mwo_NameCombination subject, mwo_NameCombination target)
        {
            Trace.Trace(nameof(OwnNameCombinationExecutable));
            target.mwo_Name = Combiner.Combine("{0} - {1}", subject.mwo_Table, subject.mwo_Column);
            Trace.Trace("Done");
        }
    }
}
