using Examine;
using Examine.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PassleSync.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Composing;
using UmbracoConstants = Umbraco.Core.Constants;

namespace PassleSync.Core.Components
{
    public class IndexNestedContentComponent : IComponent
    {
        private readonly IExamineManager _examineManager;
        private BaseIndexProvider _index;

        public IndexNestedContentComponent(IExamineManager examineManager)
        {
            _examineManager = examineManager;
        }

        public void Initialize()
        {
            if (!_examineManager.TryGetIndex(UmbracoConstants.UmbracoIndexes.ExternalIndexName, out var index))
            {
                throw new InvalidOperationException($"No index found with name {UmbracoConstants.UmbracoIndexes.ExternalIndexName}");
            }
            
            _index = (BaseIndexProvider) index;
            _index.TransformingIndexValues += TransformingIndexValues;
        }

        public void Terminate()
        {
            _index.TransformingIndexValues -= TransformingIndexValues;
        }

        public void TransformingIndexValues(object sender, IndexingItemEventArgs e)
        {
            // Extract JSON properties
            var fieldKeys = e.ValueSet.Values.Keys.ToArray();
            foreach (var key in fieldKeys)
            {
                var value = e.ValueSet.Values[key].FirstOrDefault();
                if (value is string stringValue)
                {
                    if (stringValue.DetectIsJson())
                    {
                        IndexNestedObject(e.ValueSet.Values, JsonConvert.DeserializeObject(stringValue), key);
                    }
                    else if (stringValue.DetectHasLineBreaks())
                    {
                        // Handle repeatable textstrings
                        var items = stringValue.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        var joinedItems = string.Join("|", items);
                        AddValueToField(e.ValueSet.Values, key, joinedItems, true);
                    }
                }
            }
        }

        private void IndexNestedObject(IDictionary<string, List<object>> fields, object obj, string prefix)
        {
            if (obj is JObject jObj)
            {
                foreach (var kvp in jObj)
                {
                    var propKey = prefix + "_" + kvp.Key;
                    var valueType = kvp.Value.GetType();
                    if (typeof(JContainer).IsAssignableFrom(valueType))
                    {
                        IndexNestedObject(fields, kvp.Value, propKey);
                    }
                    else
                    {
                        AddValueToField(fields, propKey, kvp.Value.ToString().StripHtml());
                    }
                }
            }
            else if (obj is JArray jArr)
            {
                for (var i = 0; i < jArr.Count; i++)
                {
                    var item = jArr[i];
                    var propKey = prefix;
                    var valueType = item.GetType();
                    if (typeof(JContainer).IsAssignableFrom(valueType))
                    {
                        IndexNestedObject(fields, item, propKey);
                    }
                    else
                    {
                        AddValueToField(fields, propKey, item.ToString().StripHtml());
                    }
                }
            }
        }
        
        private void AddValueToField(IDictionary<string, List<object>> fields, string key, string value, bool replace = false)
        {
            if (replace)
            {
                fields.Remove(key);
            }

            fields.TryGetValue(key, out var values);

            if (values == null)
            {
                values = new List<object>();
            }
            else
            {
                fields.Remove(key);
            }

            values.Add(value);
            fields.Add(key, new List<object> { string.Join("|", values) });
        }
    }
}
