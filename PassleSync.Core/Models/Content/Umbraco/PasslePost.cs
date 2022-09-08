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
        public int EstimatedReadTimeInMinutes => Math.Max((int) Math.Ceiling(EstimatedReadTimeInSeconds / 60.0), 1);
        public PostAuthor PrimaryAuthor => Authors.First();

        public string GetDate(string format)
        {
            return DateTime.Parse(PublishedDate).ToString(format);
        }

        public IEnumerable<PassleAuthor> GetAuthors()
        {
            var shortcodes = Authors.Select(x => x.Shortcode);

            return GetAuthorsForShortcodes(shortcodes);
        }

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
