using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using mwo.D365NameCombiner.Plugins.Models;
using mwo.D365NameCombiner.Plugins.Services;
using System;
using System.ServiceModel;

namespace mwo.D365NameCombiner.Plugins.Executables
{
    public class NameCombinationExecutable
    {
        private CombinerService CombinerService { get; set; }
        private ICRMContext Context { get; set; }

        private string[] configFields = new string[]
        {
            mwo_NameCombination.Fields.mwo_Column,
            mwo_NameCombination.Fields.mwo_Format,
            mwo_NameCombination.Fields.mwo_format0,
            mwo_NameCombination.Fields.mwo_format1,
            mwo_NameCombination.Fields.mwo_format2,
            mwo_NameCombination.Fields.mwo_format3,
            mwo_NameCombination.Fields.mwo_format4,
            mwo_NameCombination.Fields.mwo_format5,
            mwo_NameCombination.Fields.mwo_format6,
            mwo_NameCombination.Fields.mwo_format7,
            mwo_NameCombination.Fields.mwo_format8,
            mwo_NameCombination.Fields.mwo_format9
        };

        public NameCombinationExecutable(CombinerService combinerService, ICRMContext context)
        {
            CombinerService = combinerService;
            Context = context;
        }

        public void Execute(Entity target, string configString)
        {
            var config = GetConfig(configString);
            if (config == null)
            {
                Context.Trace.Trace($"Unable to find Name Combination \"{configString}\", skipping combination.");
                return;
            }

            Context.Trace.Trace($"Combining for {config.mwo_Column}");
            target[config.mwo_Column] = CombinerService.Combine(config.mwo_Format,
                                                                config.mwo_format0,
                                                                config.mwo_format1,
                                                                config.mwo_format2,
                                                                config.mwo_format3,
                                                                config.mwo_format4,
                                                                config.mwo_format5,
                                                                config.mwo_format6,
                                                                config.mwo_format7,
                                                                config.mwo_format8,
                                                                config.mwo_format9);
        }

        private mwo_NameCombination GetConfig(string configString)
        {
            if (Guid.TryParse(configString, out Guid configId))
            {
                try
                {
                    var config = Context.OrgService.Retrieve(
                                                    mwo_NameCombination.EntityLogicalName,
                                                    configId,
                                                    new ColumnSet(configFields));

                    return config.ToEntity<mwo_NameCombination>();
                }
                catch (FaultException<OrganizationServiceFault> e)
                {
                    Context.Trace.Trace($"Unable to retrieve Name Combination \"{configString}\": {e.Message}");
                }
            }
            else 
                Context.Trace.Trace($"Unable to parse Configuration String: {configString}");

            return null;
        }
    }
}
