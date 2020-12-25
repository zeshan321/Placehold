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

        public static string? ExtractCaptured(this string source, char start = '%', char end = '%')
        {
            var lastIsSymbol = source[source.Length - 1] == end;
            if (!lastIsSymbol || source.Length - 2 < 0)
            {
                return null;
            }

            var isUsingArgs = source.Contains('(') || source.Contains(')');
            var closed = -1;

            int count = 1;
            int startIndex = -1;
            for (var i = source.Length - 2; i >= 0; i--)
            {
                count++;
                if (isUsingArgs)
                {
                    if (source[i] == ')')
                        closed++;

                    if (source[i] == '(')
                        closed++;
                }

                if (source[i] == start && (isUsingArgs ? closed == 1 : true))
                {
                    startIndex = i;
                    break;
                }
            }

            if (startIndex == -1)
            {
                return null;
            }

            return source.Substring(startIndex, count);
        }
    }
}
