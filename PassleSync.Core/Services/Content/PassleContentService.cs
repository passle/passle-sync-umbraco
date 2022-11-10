using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;
using PassleSync.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PassleSync.Core.Services.Content
{
    public class PassleContentService<TPlural, TSingular>
        where TPlural : PaginatedResponseBase
        where TSingular : class
    {
        protected readonly ApiService _apiService;
        protected readonly ConfigService _configService;
        protected string _path = "";
        protected Func<TPlural, IEnumerable<TSingular>> _responseSelector = x => new List<TSingular>();
        protected string _itemType = "Shortcode";

        public PassleContentService(
            ApiService apiService,
            ConfigService configService)
        {
            _apiService = apiService;
            _configService = configService;
        }

        public IEnumerable<TSingular> GetAll()
        {
            IEnumerable<TSingular> result = new List<TSingular>();

            foreach (var shortcode in _configService.PassleShortcodes)
            {
                var passlePosts = Get(shortcode);
                result = result.Concat(passlePosts);
            }

            return result;
        }

        public IEnumerable<TSingular> Get(string passleShortcode)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "PassleShortcode", passleShortcode },
                { "ItemsPerPage", "100" }
            };

            return GetFromApi(queryParams);
        }

        public IEnumerable<TSingular> GetMany(string[] itemShortcodes)
        {
            var queryParams = new Dictionary<string, string>
            {
                { _itemType, string.Join(",", itemShortcodes) }
            };

            return GetFromApi(queryParams);
        }

        public TSingular GetOne(string itemShortcode)
        {
            var queryParams = new Dictionary<string, string>
            {
                { _itemType, itemShortcode }
            };

            return GetFromApi(queryParams).FirstOrDefault();
        }

        public IEnumerable<TSingular> Get(IEnumerable<string> passleShortcodes)
        {
            return Get(string.Join(",", passleShortcodes));
        }

        public IEnumerable<TSingular> GetFromApi(Dictionary<string, string> queryParams)
        {
            var url = new URLFactory()
                .Root(_configService.ApiUrl)
                .Path(_path)
                .Parameters(queryParams)
                .Build();

            var responses = _apiService.GetAllPaginatedAsync<TPlural>(url);
            var items = responses.SelectMany(_responseSelector);

            if (items.Contains(default))
            {
                throw new Exception("Failed to get data from the API");
            }

            return items;
        }
    }
}
