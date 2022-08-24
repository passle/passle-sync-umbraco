using PassleSync.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace PassleSync.Core.Services.API
{
    public class ApiService
    {
        private readonly string _apiKey = "";
        public string ApiKey { get => _apiKey; }

        public ApiService()
        {
            _apiKey = ConfigService.Passle.ClientWebAPIKey;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apiKey", _apiKey);

            var streamTask = client.GetStreamAsync(url);
            var result = await JsonSerializer.DeserializeAsync<T>(await streamTask);

            return result;
        }

        public async Task<IEnumerable<T>> GetAllPaginatedAsync<T>(string url, int pageNumber = 1)
            where T : PaginatedResponseBase
        {
            var result = new List<T>();
            var nextUrl = GetNextUrl(url, pageNumber);

            while (nextUrl != null)
            {
                var response = await GetAsync<T>(nextUrl);
                result.Add(response);

                var moreDataAvailable = response.TotalCount > (response.PageSize * response.PageNumber);
                if (moreDataAvailable)
                {
                    nextUrl = GetNextUrl(url, pageNumber + 1);
                }
                else
                {
                    nextUrl = null;
                }
            }

            return result;
        }

        private static string GetNextUrl(string url, int pageNumber)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["PageNumber"] = pageNumber.ToString();
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }
    }
}
