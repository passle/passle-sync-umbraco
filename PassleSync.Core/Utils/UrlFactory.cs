using System;
using System.Collections.Generic;
using System.Linq;

namespace PassleSync.Core.Utils
{
    public class URLFactory
    {
        private string _protocol = "https";
        private string _root = "clientwebapi.passle.localhost/api"; // TODO: Don't hardcode this URL
        private string _path = "/";
        private IDictionary<string, string> _parameters = new Dictionary<string, string>();

        public URLFactory Protocol(string protocol)
        {
            _protocol = protocol;
            return this;
        }

        public URLFactory Root(string root)
        {
            _root = root;
            return this;
        }

        public URLFactory Path(string path)
        {
            _path = $"/{path}";
            return this;
        }

        public URLFactory Parameters(Dictionary<string, string> parameters)
        {
            foreach (var key in parameters.Keys)
            {
                _parameters[key] = parameters[key];
            }

            return this;
        }

        public string Build()
        {
            if (string.IsNullOrEmpty(_root))
            {
                throw new ArgumentException("The root address cannot be null or empty");
            }

            var url = $"{_protocol}://{_root}{_path}";
            var query = string.Join("&", _parameters.Keys.Select(x => $"{Uri.EscapeUriString(x)}={Uri.EscapeUriString(_parameters[x])}"));

            return string.Join("?", url, query);
        }
    }
}
