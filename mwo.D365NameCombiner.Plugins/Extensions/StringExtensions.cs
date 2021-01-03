using System.Collections.Generic;

namespace mwo.D365NameCombiner.Plugins.Extensions
{
    public static class StringExtensions
    {
        public static Dictionary<string, string> ToDictionary(this string format, char separator = ';', char splitter = '=')
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(format)) return dict;

            var vals = format.Split(separator);

            foreach (var val in vals)
            {
                if (string.IsNullOrEmpty(val)) continue;

                var parts = val.Split(splitter);
                if (parts.Length > 1)
                    dict.Add(parts[0], parts[1]);
            }

            return dict;
        }
    }
}
