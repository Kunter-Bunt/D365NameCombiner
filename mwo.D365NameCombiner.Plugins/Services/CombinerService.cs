using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwo.D365NameCombiner.Plugins
{
    public class CombinerService
    {
        private Entity Entity { get; set; }

        public CombinerService(Entity entity)
        {
            Entity = entity;
        }

        public string Combine(string format, params string[] args)
        {
            var transformedArguments = new List<object>();

            foreach (var arg in args)
            {
                if (Entity.Contains(arg))
                    transformedArguments.Add(Entity[arg]);
                else
                    transformedArguments.Add(null);
            }

            return string.Format(format, transformedArguments.ToArray());
        }
    }
}
