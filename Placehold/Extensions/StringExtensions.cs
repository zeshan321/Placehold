using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Placehold.Extensions
{
    public static class StringExtensions
    {
        public static List<string> ExtractFromString(this string source, string start = "%", string end = "%")
        {
            var results = new List<string>();

            var pattern = string.Format("{0}({1}){2}", Regex.Escape(start), ".+?", Regex.Escape(end));
            foreach (Match match in Regex.Matches(source, pattern))
            {
                results.Add(match.Groups[1].Value);
            }

            return results;
        }
    }
}
