using System;
using System.Text.RegularExpressions;
using System.Web;

namespace PassleSync.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string FirstCharToLower(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToLower() + input.Substring(1);
            }
        }

        public static string ToPropertyAlias(this string input)
        {
            var result = input.FirstCharToLower();
            
            if (result == "name")
            {
                result = "passleName";
            }

            return result;
        }

        public static string FromPascalCaseToTitleCase(this string input)
        {
            return Regex.Replace(input, "[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1]);
        }

        public static string UrlEncode(this string input)
        {
            return HttpUtility.UrlEncode(input.Replace("'", "%27"));
        }

        public static string Truncate(this string input)
        {
            if (input.Length < 120)
            {
                return input;
            }
            return input.Substring(0, 120) + "...";
        }

        public static bool DetectHasLineBreaks(this string input)
        {
            return input.IndexOf("\r\n") > -1;
        }
    }
}
