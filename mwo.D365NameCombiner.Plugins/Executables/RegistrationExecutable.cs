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
        private ITracingService Trace;

        public RegistrationExecutable(ICRMContext context)
        {
            Context = context;
            Trace = context.Trace;
        }

        public void Execute(mwo_NameCombination subject)
        {
            Trace.Trace(nameof(RegistrationExecutable));
            var updateEntity = new mwo_NameCombination(subject.Id);

            Trace.Trace(nameof(subject.mwo_CreateStep));
            if (subject.mwo_CreateStep != null)
                UpdateCreateStep(subject);
            else
                updateEntity.mwo_CreateStep = CreateCreateStep(subject);

            Trace.Trace(nameof(subject.mwo_UpdateStep));
            if (subject.mwo_UpdateStep != null)
                UpdateUpdateStep(subject);
            else
                updateEntity.mwo_UpdateStep = CreateUpdateStep(subject);

            Trace.Trace($"Update needed: {updateEntity.Attributes.Count}");
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
            Trace.Trace($"Composing Entity for {message}, {filters}, {subject.Id}");
            Trace.Trace($"State {subject.StateCode}, Status {subject.StatusCode}");

            var isUpdate = message == "Update";
            var isActive = subject.StateCode == mwo_NameCombinationState.Active;
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
                mwo_FilteringAttributes = filters, 
                mwo_ImageType = isUpdate ? mwo_ImageType.PreImage : mwo_ImageType.None,
                mwo_ImageName = CRMPluginContext.PreImageName,
                mwo_ImageAttributes = filters,
                StateCode = isActive ? mwo_PluginStepRegistrationState.Active : mwo_PluginStepRegistrationState.Inactive,
                StatusCode = isActive ? mwo_PluginStepRegistration_StatusCode.Active : mwo_PluginStepRegistration_StatusCode.Inactive,
            };
            Trace.Trace($"Composed based Entity");

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
            Trace.Trace($"Getting Metadata for {table}");
            var meta = GetMetadata(table);

            foreach (var arg in args)
            {
                Trace.Trace($"Evaluating {arg}");
                if (string.IsNullOrEmpty(arg))
                    continue;
                else if (arg.Contains("=>"))
                    strings.AddRange(arg.Split('"').Where(_ => MetaDataHas(meta, _)));
                else if (MetaDataHas(meta, arg))
                    strings.Add(arg);
            }
            Trace.Trace($"Filters:");
            strings.ForEach(_ => Trace.Trace(_));

            var filters = string.Join(",", strings.ToArray());
            return filters;
        }

        private bool MetaDataHas(IEnumerable<AttributeMetadata> meta, string arg) => meta.Any(_ => _.LogicalName == arg);

        private IEnumerable<AttributeMetadata> GetMetadata(string table)
        {
            var meta = (RetrieveEntityResponse)Context.OrgService.Execute(new RetrieveEntityRequest()
            {
                LogicalName = table,
                EntityFilters = EntityFilters.Attributes
            });

            return meta.EntityMetadata.Attributes.ToList();
        }
    }
}
