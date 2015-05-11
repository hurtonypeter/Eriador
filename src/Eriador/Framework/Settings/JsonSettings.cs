using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;

namespace Eriador.Framework.Settings
{
    public class JsonSetting : IConfiguration
    {
        private readonly IList<JsonSettingsSource> _sources = new List<JsonSettingsSource>();

        public string this[string key]
        {
            get
            {
                return Get(key);
            }

            set
            {
                Set(key, value);
            }
        }

        public IEnumerable<IConfigurationSource> Sources
        {
            get
            {
                return _sources;
            }
        }

        public JsonSetting Add(JsonSettingsSource configurationSource)
        {
            return Add(configurationSource, load: true);
        }
        public JsonSetting Add(JsonSettingsSource configurationSource, bool load)
        {
            if (load)
            {
                configurationSource.Load();
            }
            _sources.Add(configurationSource);
            return this;
        }

        public string Get(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            string value;
            return TryGet(key, out value) ? value : null;
        }

        public void Set(string key, string value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");

            foreach (var src in _sources)
            {
                src.Set(key, value);
                src.Save();
            }

        }

        public bool TryGet(string key, out string value)
        {
            if (key == null) throw new ArgumentNullException("key");

            // If a key in the newly added configuration source is identical to a key in a 
            // formerly added configuration source, the new one overrides the former one.
            // So we search in reverse order, starting with latest configuration source.
            foreach (var src in _sources.Reverse())
            {
                if (src.TryGet(key, out value))
                {
                    return true;
                }
            }
            value = null;
            return false;
        }

        public IConfiguration GetSubKey(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, IConfiguration>> GetSubKeys()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, IConfiguration>> GetSubKeys(string key)
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            foreach (var src in _sources)
            {
                src.Load();
            }
        }
    }
}
