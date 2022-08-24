using System;
using System.Text.RegularExpressions;

namespace PassleSync.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToTelephoneLink(this string phoneNumber)
        {
            return string.Format("tel:{0}", Regex.Replace(phoneNumber, @"[^0-9]", ""));
        }

        public static string RemoveParagraphWrapperTags(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string trimmedText = text.Trim();
            string upperText = trimmedText.ToUpper();
            int paragraphIndex = upperText.IndexOf("<P>");

            if (paragraphIndex != 0 ||
                paragraphIndex != upperText.LastIndexOf("<P>") ||
                upperText.Substring(upperText.Length - 4, 4) != "</P>")
            {
                // Paragraph not used as a wrapper element
                return text;
            }

            // Remove paragraph wrapper tags
            return trimmedText.Substring(3, trimmedText.Length - 7);
        }

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
    }      
}
