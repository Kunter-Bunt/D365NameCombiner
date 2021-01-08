using mwo.D365NameCombiner.Plugins.Models;
using System.Linq.Dynamic.Core;

namespace mwo.D365NameCombiner.Plugins.Services
{
    public class ExpressionConverterService
    {
        public ICRMContext Context;

        public ExpressionConverterService(ICRMContext context)
        {
            Context = context;
        }

        public object Convert(string expression)
        {
            var exp = DynamicExpressionParser.ParseLambda<ICRMContext, object>(new ParsingConfig(), true, expression);
            return exp.Compile().Invoke(Context);
        }
    }
}
