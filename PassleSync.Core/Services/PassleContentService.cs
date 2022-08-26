﻿using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;
using PassleSync.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PassleSync.Core.Services.Content
{
    public class PassleContentService
    {
        protected readonly ApiService _apiService;
        protected readonly ConfigService _configService;

        protected IEnumerable<PasslePost> _passlePosts;
        public IEnumerable<PasslePost> PasslePosts { get => _passlePosts; }

        protected IEnumerable<PassleAuthor> _passleAuthors;
        public IEnumerable<PassleAuthor> PassleAuthors { get => _passleAuthors; }

        public PassleContentService(
            ApiService apiService,
            ConfigService configService)
        {
            _apiService = apiService;
            _configService = configService;
        }

        public async Task<IEnumerable<PasslePost>> GetPasslePosts()
        {
            IEnumerable<PasslePost> result = new List<PasslePost>();

            foreach (var shortcode in _configService.PassleShortcodes)
            {
                var passlePosts = await GetPosts(shortcode);
                result = result.Concat(passlePosts);
            }

            _passlePosts = result;
            return result;
        }

        public async Task<IEnumerable<PasslePost>> GetPosts(string passleShortcode)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "PassleShortcode", passleShortcode },
                { "ItemsPerPage", "100" }
            };

            return await GetPostsFromApi(queryParams);
        }

        public async Task<IEnumerable<PasslePost>> GetPosts(IEnumerable<string> postShortcodes)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "PostShortcode", string.Join(",", postShortcodes) },
                { "ItemsPerPage", "100" }
            };

            return await GetPostsFromApi(queryParams);
        }

        public async Task<IEnumerable<PasslePost>> GetPostsFromApi(Dictionary<string, string> queryParams)
        {
            var url = new URLFactory()
                .Root(_configService.ApiUrl)
                .Path("/passlesync/posts")
                .Parameters(queryParams)
                .Build();

            var responses = await _apiService.GetAllPaginatedAsync<PasslePosts>(url);
            var posts = responses.SelectMany(x => x.Posts);

            if (posts.Contains(null))
            {
                throw new Exception("Failed to get data from the API");
            }

            return posts;
        }

        public async Task<IEnumerable<PassleAuthor>> GetPassleAuthors()
        {
            IEnumerable<PassleAuthor> result = new List<PassleAuthor>();

            foreach (var shortcode in _configService.PassleShortcodes)
            {
                var passleAuthors = await GetAuthors(shortcode);
                result = result.Concat(passleAuthors);
            }

            _passleAuthors = result;
            return result;
        }

        public async Task<IEnumerable<PassleAuthor>> GetAuthors(string passleShortcode)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "PassleShortcode", passleShortcode },
                { "ItemsPerPage", "100" }
            };

            return await GetAuthorsFromApi(queryParams);
        }

        public async Task<IEnumerable<PassleAuthor>> GetAuthors(IEnumerable<string> authorShortcodes)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "PersonShortcode", string.Join(",", authorShortcodes) },
                { "ItemsPerPage", "100" }
            };

            return await GetAuthorsFromApi(queryParams);
        }

        public async Task<IEnumerable<PassleAuthor>> GetAuthorsFromApi(Dictionary<string, string> queryParams)
        {
            var url = new URLFactory()
                .Path("/passlesync/people")
                .Parameters(queryParams)
                .Build();

            var responses = await _apiService.GetAllPaginatedAsync<PassleAuthors>(url);
            var authors = responses.SelectMany(x => x.People);

            if (authors.Contains(null))
            {
                throw new Exception("Failed to get data from the API");
            }

            return authors;
        }
    }
}
