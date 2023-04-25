using PassleSync.Core.Models.Content.PassleApi;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;

namespace PassleSync.Core.Services.API
{
    public class ApiService
    {
        private readonly ConfigService _configService;

        public ApiService(ConfigService configService)
        {
            _configService = configService;
        }

        public IEnumerable<T> GetAllPaginatedAsync<T>(string url, int pageNumber = 1)
            where T : PaginatedResponseBase
        {
            var result = new List<T>();
            var nextUrl = GetNextUrl(url, pageNumber);

            while (nextUrl != null)
            {
                var response = GetAsync<T>(nextUrl);
                result.Add(response);

                var moreDataAvailable = response.TotalCount > (response.PageSize * response.PageNumber);
                if (moreDataAvailable)
                {
                    pageNumber += 1;
                    nextUrl = GetNextUrl(url, pageNumber);
                }
                else
                {
                    nextUrl = null;
                }
            }

            return result;
        }

        private T GetAsync<T>(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apiKey", _configService.ClientApiKey);
            client.DefaultRequestHeaders.Add("X-PassleSimulateRemoteHosting", _configService.SimulateRemoteHosting.ToString());

            if (_configService.SimulateRemoteHosting)
            {
                client.DefaultRequestHeaders.Add("X-PassleRemoteHostingUseHttps", _configService.UseHttps.ToString());

                if (!string.IsNullOrEmpty(_configService.CustomDomain))
                {
                    client.DefaultRequestHeaders.Add("X-PassleRemoteHostingCustomDomain", _configService.CustomDomain);
                }
                if (!string.IsNullOrEmpty(_configService.PasslePermalinkPrefix))
                {
                    client.DefaultRequestHeaders.Add("X-PassleRemoteHostingPasslePath", _configService.PasslePermalinkPrefix);
                }
                if (!string.IsNullOrEmpty(_configService.PostPermalinkPrefix))
                {
                    client.DefaultRequestHeaders.Add("X-PassleRemoteHostingPostPath", _configService.PostPermalinkPrefix);
                }
                if (!string.IsNullOrEmpty(_configService.AuthorPermalinkPrefix))
                {
                    client.DefaultRequestHeaders.Add("X-PassleRemoteHostingProfilePath", _configService.AuthorPermalinkPrefix);
                }
            }

            var response = client.GetAsync(url).Result;
            var result = response.Content.ReadAsAsync<T>().Result;

            return result;
        }

        private string GetNextUrl(string url, int pageNumber)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            query["PageNumber"] = pageNumber.ToString();
            uriBuilder.Query = query.ToString();

            return uriBuilder.ToString();
        }
    }
}
