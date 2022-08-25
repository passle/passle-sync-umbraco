using System;
using System.Net.Http.Headers;
using System.Net.Http;
using Umbraco.Core.Services;
using Umbraco.Web.Composing;
using Umbraco.Core;
using PassleSync.Core.Models.Content.PassleApi;

namespace PassleSync.Core.Helpers
{
    public static class ApiHelper
    {
        public static PasslePosts GetPosts()
        {
            IKeyValueService kv = Current.Factory.GetInstance<IKeyValueService>();
            var result = new PasslePosts();

            HttpClient client = new HttpClient();
            string baseApiAddress = kv.GetValue("PassleSync.ApiUrl");

            client.BaseAddress = new Uri(baseApiAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var apiKey = kv.GetValue("PassleSync.ApiKey");
            client.DefaultRequestHeaders.Add("apiKey", apiKey);

            var shortCode = kv.GetValue("PassleSync.Shortcode");
            HttpResponseMessage response = client.GetAsync("/api/v2/passlesync/posts?PassleShortcode=" + shortCode + "&ItemsPerPage=100").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<PasslePosts>().Result;
            }

            return result;
        }

        public static PassleAuthors GetAuthors()
        {
            IKeyValueService kv = Current.Factory.GetInstance<IKeyValueService>();
            var result = new PassleAuthors();

            HttpClient client = new HttpClient();
            string baseApiAddress = kv.GetValue("PassleSync.ApiUrl");

            client.BaseAddress = new Uri(baseApiAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var apiKey = kv.GetValue("PassleSync.ApiKey");
            client.DefaultRequestHeaders.Add("apiKey", apiKey);

            var shortCode = kv.GetValue("PassleSync.Shortcode");
            HttpResponseMessage response = client.GetAsync("/api/v2/passlesync/people?PassleShortcode=" + shortCode + "&ItemsPerPage=100").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<PassleAuthors>().Result;
            }

            return result;
        }
    }
}
