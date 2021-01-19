using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using mwo.D365NameCombiner.Plugins.EntryPoints;
using mwo.D365NameCombiner.Plugins.Models;
using System.Collections.Generic;
using System.Linq;

namespace mwo.D365NameCombiner.Plugins.Executables
{
    public class RegistrationExecutable
    {
        private ICRMContext Context;

        public RegistrationExecutable(ICRMContext context)
        {
            Context = context;
        }

        public void Execute(mwo_NameCombination subject)
        {
            var updateEntity = new mwo_NameCombination(subject.Id);

            if (subject.mwo_CreateStep != null)
                UpdateCreateStep(subject);
            else
                updateEntity.mwo_CreateStep = CreateCreateStep(subject);

            if (subject.mwo_UpdateStep != null)
                UpdateUpdateStep(subject);
            else
                updateEntity.mwo_UpdateStep = CreateUpdateStep(subject);

            if (updateEntity.Attributes.Any())
                Context.OrgService.Update(updateEntity);
        }

        private EntityReference CreateCreateStep(mwo_NameCombination subject)
        {
            return CreateStep(subject, "Create", null);
        }

        private EntityReference CreateUpdateStep(mwo_NameCombination subject)
        {
            return CreateStep(subject, "Update", GetFilters(subject));
        }

        private EntityReference CreateStep(mwo_NameCombination subject, string message, string filters)
        {
            var step = ComposeEntity(subject, message, filters);
            step.Id = Context.OrgService.Create(step);
            return step.ToEntityReference();
        }

        private void UpdateCreateStep(mwo_NameCombination subject)
        {
            UpdateStep(subject, "Create", null, subject.mwo_CreateStep);
        }

        private void UpdateUpdateStep(mwo_NameCombination subject)
        {
            UpdateStep(subject, "Update", GetFilters(subject), subject.mwo_UpdateStep);
        }

        private void UpdateStep(mwo_NameCombination subject, string message, string filters, EntityReference existingStep)
        {
            var step = ComposeEntity(subject, message, filters);
            step.Id = existingStep.Id;
            Context.OrgService.Update(step);
        }

        private mwo_PluginStepRegistration ComposeEntity(mwo_NameCombination subject, string message, string filters)
        {
            var step = new mwo_PluginStepRegistration
            {
                mwo_EventHandler = typeof(NameCombiner).FullName,
                mwo_EventHandlerType = mwo_EventHandlerType.PluginType,
                mwo_Name = $"{nameof(NameCombiner)}_{subject.mwo_Table}_{subject.mwo_Column}_{message}",
                mwo_Managed = true,
                mwo_SDKMessage = message,
                mwo_PrimaryEntity = subject.mwo_Table,
                mwo_PluginStepStage = mwo_PluginStage.PreOperation,
                mwo_Asynchronous = false,
                mwo_StepConfiguration = subject.Id.ToString(),
                mwo_FilteringAttributes = filters
            };
            return step;
        }

        private string GetFilters(mwo_NameCombination subject)
        {
            return GetFilters(subject.mwo_Table,
                            subject.mwo_format0,
                            subject.mwo_format1,
                            subject.mwo_format2,
                            subject.mwo_format3,
                            subject.mwo_format4,
                            subject.mwo_format5,
                            subject.mwo_format6,
                            subject.mwo_format7,
                            subject.mwo_format8,
                            subject.mwo_format9);
        }

        private string GetFilters(string table, params string[] args)
        {
            var strings = new List<string>();
            var meta = GetMetadata(table);

            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                    continue;
                else if (arg.Contains("=>"))
                    strings.AddRange(arg.Split('"').Where(_ => MetaDataHas(meta, _)));
                else if (MetaDataHas(meta, arg))
                    strings.Add(arg);
            }

            return string.Join(",", strings.ToArray());
        }

        private bool MetaDataHas(IEnumerable<AttributeMetadata> meta, string arg) => meta.Any(_ => _.LogicalName == arg);

        private IEnumerable<AttributeMetadata> GetMetadata(string table)
        {
            var meta = (RetrieveEntityResponse)Context.OrgService.Execute(new RetrieveEntityRequest()
            {
                LogicalName = table
            });

            return meta.EntityMetadata.Attributes.ToList();
        }
    }
}
