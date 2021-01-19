using Microsoft.Xrm.Sdk;
using mwo.D365NameCombiner.Plugins.Models;
using System.Collections.Generic;

namespace mwo.D365NameCombiner.Plugins.Services
{
    public class CombinerService
    {
        private Entity Entity { get; set; }
        private AttributeConverterService AttributeService { get; set; }
        private ExpressionConverterService ExpressionService { get; set; }
        public ICRMContext Context { get; }

        public CombinerService(Entity entity, AttributeConverterService attributeService, ExpressionConverterService expressionService, ICRMContext context)
        {
            Entity = entity;
            AttributeService = attributeService;
            ExpressionService = expressionService;
            Context = context;
        }

        public string Combine(string format, params string[] args)
        {
            var transformedArguments = new List<object>();

            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                    continue;

                Context.Trace.Trace($"Processing arg: {arg}");

                object transformed = null;
                if (arg.Contains("=>"))
                    transformed = ExpressionService.Convert(arg);
                else
                    transformed = AttributeService.Convert(Entity, arg);

                Context.Trace.Trace($"Processed arg: {transformed}");
                transformedArguments.Add(transformed);
            }

            Context.Trace.Trace($"Done transforming, formatting \"{format}\" with {transformedArguments.Count} args.");

            return string.Format(format, transformedArguments.ToArray());
        }
    }
}
