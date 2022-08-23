using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Umbraco.Core.Services;
using Umbraco.Web.Composing;
using Umbraco.Core;

namespace PassleSync.Core.Helpers
{
    public static class ApiHelper
    {
        public static PassleSync.Core.Models.Admin.PaginatedResponse GetPosts()
        {
            IKeyValueService kv = Current.Factory.GetInstance<IKeyValueService>();
            var result = new PassleSync.Core.Models.Admin.PaginatedResponse();

            HttpClient client = new HttpClient();
            string baseApiAddress = kv.GetValue("Passle.apiUrl");

            client.BaseAddress = new Uri(baseApiAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var apiKey = kv.GetValue("Passle.apiKey");
            client.DefaultRequestHeaders.Add("apiKey", apiKey);

            var shortCode = kv.GetValue("Passle.shortcode");
            HttpResponseMessage response = client.GetAsync("/api/v2/passlesync/posts?PassleShortcode=" + shortCode + "&ItemsPerPage=100").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<PassleSync.Core.Models.Admin.PaginatedResponse>().Result;
            }

            return result;
        }

        public static PassleSync.Core.Models.Admin.PaginatedResponse GetAuthors()
        {
            IKeyValueService kv = Current.Factory.GetInstance<IKeyValueService>();
            var result = new PassleSync.Core.Models.Admin.PaginatedResponse();

            HttpClient client = new HttpClient();
            string baseApiAddress = kv.GetValue("Passle.apiUrl");

            client.BaseAddress = new Uri(baseApiAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var apiKey = kv.GetValue("Passle.apiKey");
            client.DefaultRequestHeaders.Add("apiKey", apiKey);

            var shortCode = kv.GetValue("Passle.shortcode");
            HttpResponseMessage response = client.GetAsync("/api/v2/passlesync/people?PassleShortcode=" + shortCode + "&ItemsPerPage=100").Result;
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsAsync<PassleSync.Core.Models.Admin.PaginatedResponse>().Result;
            }

            return result;
        }
    }
}
