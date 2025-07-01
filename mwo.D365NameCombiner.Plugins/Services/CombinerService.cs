using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Models;
using System.Collections.Generic;

namespace mwo.D365NameCombiner.Plugins.Services
{
    public class CombinerService
    {
        private Entity Entity { get; set; }
        private AttributeConverterService AttributeService { get; set; }
        public ICRMContext Context { get; }

        public CombinerService(Entity entity, AttributeConverterService attributeService, ICRMContext context)
        {
            Entity = entity;
            AttributeService = attributeService;
            Context = context;
        }

        public string Combine(string format, params string[] args)
        {
            var transformedArguments = new List<object>();

            Context.Trace.Trace($"Transforming:");
            foreach (var arg in args)
            {
                object transformed = null;

                Context.Trace.Trace($"Processing arg: {arg}");
                if (string.IsNullOrEmpty(arg))
                    transformed = null;
                else
                    transformed = AttributeService.Convert(Entity, arg);

                Context.Trace.Trace($"Processed arg: {transformed}");
                transformedArguments.Add(transformed);

                Context.Trace.Trace($"-----------------");
            }

            Context.Trace.Trace($"Done transforming, formatting \"{format}\" with {transformedArguments.Count} args.");

            return string.Format(format, transformedArguments.ToArray());
        }
    }
}
