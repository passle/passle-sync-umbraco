//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v8.1.0
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder;
using Umbraco.ModelsBuilder.Umbraco;

namespace PassleSync.Core.Models.Content.Umbraco
{
	/// <summary>Passle Post</summary>
	[PublishedModel("passlePost")]
	public partial class PasslePost : PublishedContentModel
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		public new const string ModelTypeAlias = "passlePost";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		public new static IPublishedContentType GetModelContentType()
			=> PublishedModelUtility.GetModelContentType(ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<PasslePost, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(), selector);
#pragma warning restore 0109

		// ctor
		public PasslePost(IPublishedContent content)
			: base(content)
		{ }

		// properties

		///<summary>
		/// Authors
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("authors")]
		public IEnumerable<PostAuthor> Authors => this.Value<IEnumerable<PostAuthor>>("authors");

		///<summary>
		/// CoAuthors
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("coAuthors")]
		public IEnumerable<PostAuthor> CoAuthors => this.Value<IEnumerable<PostAuthor>>("coAuthors");

		///<summary>
		/// ContentTextSnippet
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("contentTextSnippet")]
		public string ContentTextSnippet => this.Value<string>("contentTextSnippet");

		///<summary>
		/// EstimatedReadTimeInSeconds
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("estimatedReadTimeInSeconds")]
		public int EstimatedReadTimeInSeconds => this.Value<int>("estimatedReadTimeInSeconds");

		///<summary>
		/// FeaturedItemEmbedProvider
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("featuredItemEmbedProvider")]
		public string FeaturedItemEmbedProvider => this.Value<string>("featuredItemEmbedProvider");

		///<summary>
		/// FeaturedItemEmbedType
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("featuredItemEmbedType")]
		public string FeaturedItemEmbedType => this.Value<string>("featuredItemEmbedType");

		///<summary>
		/// FeaturedItemHtml
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("featuredItemHtml")]
		public string FeaturedItemHtml => this.Value<string>("featuredItemHtml");

		///<summary>
		/// FeaturedItemMediaType
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("featuredItemMediaType")]
		public string FeaturedItemMediaType => this.Value<string>("featuredItemMediaType");

		///<summary>
		/// FeaturedItemPosition
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("featuredItemPosition")]
		public string FeaturedItemPosition => this.Value<string>("featuredItemPosition");

		///<summary>
		/// ImageUrl
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("imageUrl")]
		public string ImageUrl => this.Value<string>("imageUrl");

		///<summary>
		/// IsFeaturedOnPasslePage
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("isFeaturedOnPasslePage")]
		public bool IsFeaturedOnPasslePage => this.Value<bool>("isFeaturedOnPasslePage");

		///<summary>
		/// IsFeaturedOnPostPage
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("isFeaturedOnPostPage")]
		public bool IsFeaturedOnPostPage => this.Value<bool>("isFeaturedOnPostPage");

		///<summary>
		/// IsRepost
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("isRepost")]
		public bool IsRepost => this.Value<bool>("isRepost");

		///<summary>
		/// OpensInNewTab
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("opensInNewTab")]
		public bool OpensInNewTab => this.Value<bool>("opensInNewTab");

		///<summary>
		/// PassleShortcode
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("passleShortcode")]
		public string PassleShortcode => this.Value<string>("passleShortcode");

		///<summary>
		/// PostContentHtml
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("postContentHtml")]
		public string PostContentHtml => this.Value<string>("postContentHtml");

		///<summary>
		/// PostShortcode
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("postShortcode")]
		public string PostShortcode => this.Value<string>("postShortcode");

		///<summary>
		/// PostTitle
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("postTitle")]
		public string PostTitle => this.Value<string>("postTitle");

		///<summary>
		/// PostUrl
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("postUrl")]
		public string PostUrl => this.Value<string>("postUrl");

		///<summary>
		/// PublishedDate
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("publishedDate")]
		public string PublishedDate => this.Value<string>("publishedDate");

		///<summary>
		/// QuoteText
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("quoteText")]
		public string QuoteText => this.Value<string>("quoteText");

		///<summary>
		/// QuoteUrl
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("quoteUrl")]
		public string QuoteUrl => this.Value<string>("quoteUrl");

		///<summary>
		/// ShareViews
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("shareViews")]
		public IEnumerable<PostShareViews> ShareViews => this.Value<IEnumerable<PostShareViews>>("shareViews");

		///<summary>
		/// Tags
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("tags")]
		public IEnumerable<string> Tags => this.Value<IEnumerable<string>>("tags");

		///<summary>
		/// TotalLikes
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("totalLikes")]
		public int TotalLikes => this.Value<int>("totalLikes");

		///<summary>
		/// TotalShares
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("totalShares")]
		public int TotalShares => this.Value<int>("totalShares");

		///<summary>
		/// Tweets
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder", "8.1.0")]
		[ImplementPropertyType("tweets")]
		public IEnumerable<PostTweet> Tweets => this.Value<IEnumerable<PostTweet>>("tweets");
	}
}
