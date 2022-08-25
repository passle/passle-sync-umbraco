using System.Collections.Generic;

namespace PassleSync.Core.Models.Content.PassleApi
{
    public class PasslePost
    {
        public static string PostTitleProperty { get => "PostTitle"; }
        public string PostTitle { get; set; }
        public static string PostShortcodeProperty { get => "PostShortcode"; }
        public string PostShortcode { get; set; }
        public static string PassleShortcodeProperty { get => "PassleShortcode"; }
        public string PassleShortcode { get; set; }
        public static string PublishedDateProperty { get => "PublishedDate"; }
        public string PublishedDate { get; set; }
        public static string PostUrlProperty { get => "PostUrl"; }
        public string PostUrl { get; set; }
        public static string ImageUrlProperty { get => "ImageUrl"; }
        public string ImageUrl { get; set; }
        public static string AuthorsProperty { get => "Authors"; }
        public IEnumerable<PostAuthor> Authors { get; set; }
        public static string CoAuthorsProperty { get => "CoAuthors"; }
        public IEnumerable<PostAuthor> CoAuthors { get; set; }
        public static string PostContentHtmlProperty { get => "PostContentHtml"; }
        public string PostContentHtml { get; set; }
        public static string ContentTextSnippetProperty { get => "ContentTextSnippet"; }
        public string ContentTextSnippet { get; set; }
        public static string QuoteTextProperty { get => "QuoteText"; }
        public string QuoteText { get; set; }
        public static string QuoteUrlProperty { get => "QuoteUrl"; }
        public string QuoteUrl { get; set; }
        public static string IsRepostProperty { get => "IsRepost"; }
        public bool IsRepost { get; set; }
        public static string IsFeaturedOnPasslePageProperty { get => "IsFeaturedOnPasslePage"; }
        public bool IsFeaturedOnPasslePage { get; set; }
        public static string IsFeaturedOnPostPageProperty { get => "IsFeaturedOnPostPage"; }
        public bool IsFeaturedOnPostPage { get; set; }
        public static string EstimatedReadTimeInSecondsProperty { get => "EstimatedReadTimeInSeconds"; }
        public int? EstimatedReadTimeInSeconds { get; set; }
        public static string FeaturedItemHtmlProperty { get => "FeaturedItemHtml"; }
        public string FeaturedItemHtml { get; set; }
        public static string FeaturedItemPositionProperty { get => "FeaturedItemPosition"; }
        public PostFeaturedItemPosition FeaturedItemPosition { get; set; }
        public static string FeaturedItemMediaTypeProperty { get => "FeaturedItemMediaType"; }
        public PostMediaType FeaturedItemMediaType { get; set; }
        public static string FeaturedItemEmbedTypeProperty { get => "FeaturedItemEmbedType"; }
        public PostIframelyObjectType FeaturedItemEmbedType { get; set; }
        public static string FeaturedItemEmbedProviderProperty { get => "FeaturedItemEmbedProvider"; }
        public string FeaturedItemEmbedProvider { get; set; }
        public static string TagsProperty { get => "Tags"; }
        public IEnumerable<string> Tags { get; set; }
        public static string TweetsProperty { get => "Tweets"; }
        public IEnumerable<PostTweet> Tweets { get; set; }
        public static string ShareViewsProperty { get => "ShareViews"; }
        public IEnumerable<PostShareViews> ShareViews { get; set; }
        public static string TotalSharesProperty { get => "TotalShares"; }
        public int TotalShares { get; set; }
        public static string TotalLikesProperty { get => "TotalLikes"; }
        public int TotalLikes { get; set; }
        public static string OpensInNewTabProperty { get => "OpensInNewTab"; }
        public bool OpensInNewTab { get; set; }
    }
}
