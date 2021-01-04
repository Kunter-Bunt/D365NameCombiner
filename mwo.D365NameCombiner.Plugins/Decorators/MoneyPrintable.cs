using Microsoft.Xrm.Sdk;
using System;

namespace mwo.D365NameCombiner.Plugins.Decorators
{
    public class MoneyPrintable : IFormattable
    {
        public Money Mon { get; }

        public MoneyPrintable(Money mon)
        {
            Mon = mon;
        }

        public override string ToString()
        {
            return Mon?.Value.ToString();
        }

        public string ToString(string format)
        {
            return Mon?.Value.ToString(format);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Mon?.Value.ToString(format, formatProvider);
        }
    }
}
