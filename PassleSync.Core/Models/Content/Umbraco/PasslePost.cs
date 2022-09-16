using PassleSync.Core.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace PassleSync.Core.Models.Content.Umbraco
{
    public partial class PasslePost
    {
        /// <summary>
        /// EstimatedReadTimeInMinutes
        /// </summary>
        public int EstimatedReadTimeInMinutes => Math.Max((int) Math.Ceiling(EstimatedReadTimeInSeconds / 60.0), 1);
        /// <summary>
        /// PrimaryAuthor
        /// </summary>
        public PostAuthor PrimaryAuthor => Authors.First();

        /// <summary>
        /// Get the formatted date that the post was published.
        /// </summary>
        /// <param name="format">A standard C# datetime format string</param>
        /// <seealso href="https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings"/>
        public string GetDate(string format)
        {
            return DateTime.Parse(PublishedDate).ToString(format);
        }

        /// <summary>
        /// Get full <see cref="PassleAuthor"/> models for the post authors that have been synced.
        /// </summary>
        public IEnumerable<PassleAuthor> GetAuthors()
        {
            var shortcodes = Authors.Select(x => x.Shortcode);

            return GetAuthorsForShortcodes(shortcodes);
        }

        /// <summary>
        /// Get full <see cref="PassleAuthor"/> models for the post co-authors that have been synced.
        /// </summary>
        public IEnumerable<PassleAuthor> GetCoAuthors()
        {
            var shortcodes = CoAuthors.Select(x => x.Shortcode);

            return GetAuthorsForShortcodes(shortcodes);
        }

        private IEnumerable<PassleAuthor> GetAuthorsForShortcodes(IEnumerable<string> shortcodes)
        {
            var passleHelperService = Current.Factory.GetInstance<IPassleHelperService>();
            return passleHelperService.GetAuthors().ByShortcodes(shortcodes).Execute().Items;
        }
    }
}
