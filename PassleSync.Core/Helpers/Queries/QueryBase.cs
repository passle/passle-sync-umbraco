using Examine;
using Examine.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Examine;
using Umbraco.Web;
using UmbracoConstants = Umbraco.Core.Constants;

namespace PassleSync.Core.Helpers.Queries
{
    public abstract class QueryBase<T, P>
        where T : QueryBase<T, P>
        where P : PublishedContentModel
    {
        protected readonly IExamineManager _examineManager;
        protected readonly UmbracoHelper _umbracoHelper;
        protected IBooleanOperation _query;

        protected abstract string ContentType { get; }
        protected abstract string[] SearchFields { get; }

        protected int CurrentPage { get; set; } = 1;
        protected int ItemsPerPage { get; set; } = 20;

        public QueryBase(IExamineManager examineManager, UmbracoHelper umbracoHelper)
        {
            _examineManager = examineManager;
            _umbracoHelper = umbracoHelper;

            _query = CreateQuery(ContentType);
        }

        public T Search(string searchQuery)
        {
            _query = _query.And().GroupedOr(SearchFields, searchQuery);
            return (T) this;
        }

        public T WithCurrentPage(int currentPage)
        {
            CurrentPage = currentPage;
            return (T) this;
        }

        public T WithItemsPerPage(int itemsPerPage)
        {
            ItemsPerPage = itemsPerPage;
            return (T) this;
        }

        public QueryResult<P> Execute()
        {
            var searchResults = _query.Execute();
            var ids = searchResults
                .Skip((CurrentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .Select(x => int.Parse(x.Id));

            return new QueryResult<P>()
            {
                Items = GetInstances(ids),
                CurrentPage = CurrentPage,
                ItemsPerPage = ItemsPerPage,
                TotalItems = (int) searchResults.TotalItemCount,
                TotalPages = (int) Math.Ceiling((double) searchResults.TotalItemCount / ItemsPerPage),
            };
        }

        protected IEnumerable<P> GetInstances(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                var instance = (P) Activator.CreateInstance(typeof(P), new object[] { _umbracoHelper.Content(id) });
                yield return instance;
            }
        }

        protected IBooleanOperation CreateQuery(string contentType)
        {
            if (!_examineManager.TryGetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName, out var index))
            {
                throw new InvalidOperationException($"No index found with name {UmbracoConstants.UmbracoIndexes.ExternalIndexName}");
            }

            return index
                .GetSearcher()
                .CreateQuery("content")
                .NodeTypeAlias(contentType);
        }
    }
}
