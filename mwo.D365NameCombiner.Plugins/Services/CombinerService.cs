using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace mwo.D365NameCombiner.Plugins.Services
{
    public class CombinerService
    {
        private Entity Entity { get; set; }
        private AttributeConverterService AttributeService { get; set; }

        public CombinerService(Entity entity)
        {
            Entity = entity;
            AttributeService = new AttributeConverterService();
        }

        public string Combine(string format, params string[] args)
        {
            var transformedArguments = new List<object>();

            foreach (var arg in args)
                transformedArguments.Add(AttributeService.Convert(Entity, arg));

            return string.Format(format, transformedArguments.ToArray());
        }
    }
}
