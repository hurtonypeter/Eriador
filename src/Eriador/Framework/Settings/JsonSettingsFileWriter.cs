using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Framework.ConfigurationModel.Json;
using Newtonsoft.Json.Linq;

namespace Eriador.Framework.Settings
{
    public class JsonSettingsFileWriter
    {
        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private JsonTextWriter _writer;

        public void Write(Stream output, IDictionary<string,string> data)
        {
            _writer = new JsonTextWriter(new StreamWriter(output));
            var root = new JObject();

            foreach (var item in data)
            {
                var pathFragments = item.Key.Split(':').Reverse().ToList();
                JObject jobj = new JObject(new JProperty(pathFragments.First(), item.Value));
                pathFragments.RemoveAt(0);
                foreach (var prop in pathFragments)
                {
                    jobj = new JObject(new JProperty(prop, jobj));
                }
                root.Merge(jobj);
            }
            
            root.WriteTo(_writer);
            _writer.Flush();
        }
    }
}
