using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace PassleSync.Core.Services
{
    public static class ConfigService
    {
        private static IConfiguration Configuration
        {
            get
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("passle.json", optional: false, reloadOnChange: true)
                    .Build();
            }
        }

        public static PassleConfig Passle
        {
            get
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("passle.json", optional: false, reloadOnChange: true)
                    .Build();
                return configuration.GetSection("Passle").Get<PassleConfig>();
            }
        }
    }

    public class PassleConfig
    {
        public string APIKey { get; set; }
        public string ClientWebAPIKey { get; set; }
        public IEnumerable<string> PassleShortcodes { get; set; }
        public string PostRootNode { get; set; }
        public string AuthorRootNode { get; set; }
        public int PostRootNodeId
        {
            get
            {
                try
                {
                    return int.Parse(PostRootNode);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
        public int AuthorRootNodeId
        {
            get
            {
                try
                {
                    return int.Parse(AuthorRootNode);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
    }
}
