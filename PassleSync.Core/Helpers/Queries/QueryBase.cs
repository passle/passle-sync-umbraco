﻿using Examine;
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
    public abstract class QueryBase<T> where T : PublishedContentModel
    {
        protected readonly IExamineManager _examineManager;
        protected readonly UmbracoHelper _umbracoHelper;
        protected IBooleanOperation _query;

        protected abstract string ContentType { get; }

        protected int CurrentPage { get; set; } = 1;
        protected int ItemsPerPage { get; set; } = 20;

        public QueryBase(IExamineManager examineManager, UmbracoHelper umbracoHelper)
        {
            _examineManager = examineManager;
            _umbracoHelper = umbracoHelper;

            _query = CreateQuery(ContentType);
        }

        public virtual QueryBase<T> Search(string searchQuery)
        {
            _query = _query.And().Field("nodeName", searchQuery);
            return this;
        }

        public QueryBase<T> WithCurrentPage(int currentPage)
        {
            CurrentPage = currentPage;
            return this;
        }

        public QueryBase<T> WithItemsPerPage(int itemsPerPage)
        {
            ItemsPerPage = itemsPerPage;
            return this;
        }

        public QueryResult<T> Execute()
        {
            var searchResults = _query.Execute();
            var ids = searchResults
                .Skip((CurrentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .Select(x => int.Parse(x.Id));

            return new QueryResult<T>()
            {
                Items = GetInstances(ids),
                CurrentPage = CurrentPage,
                ItemsPerPage = ItemsPerPage,
                TotalItems = (int) searchResults.TotalItemCount,
            };
        }

        protected IEnumerable<T> GetInstances(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                var instance = (T)Activator.CreateInstance(typeof(T), new object[] { _umbracoHelper.Content(id) });
                yield return instance;
            }
        }

        protected IBooleanOperation CreateQuery(string contentType)
        {
            if (!ExamineManager.Instance.TryGetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName, out var index))
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
