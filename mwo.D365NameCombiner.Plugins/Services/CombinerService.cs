using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace mwo.D365NameCombiner.Plugins.Services
{
    public class CombinerService
    {
        private Entity Entity { get; set; }
        private AttributeConverterService AttributeService { get; set; }
        private ExpressionConverterService ExpressionService { get; set; }

        public CombinerService(Entity entity, AttributeConverterService attributeService, ExpressionConverterService expressionService)
        {
            Entity = entity;
            AttributeService = attributeService;
            ExpressionService = expressionService;
        }

        public string Combine(string format, params string[] args)
        {
            var transformedArguments = new List<object>();

            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                    continue;
                if (arg.Contains("=>"))
                    transformedArguments.Add(ExpressionService.Convert(arg));
                else
                    transformedArguments.Add(AttributeService.Convert(Entity, arg));
            }


            return string.Format(format, transformedArguments.ToArray());
        }
    }
}
