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
    }      
}
