using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;

namespace Eriador.Framework.Settings
{
    public class Setting 
    {
        private readonly IList<JsonSettingsSource> _sources = new List<JsonSettingsSource>();

        public string this[string key]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IConfigurationSource> Sources
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Setting Add(JsonSettingsSource configurationSource)
        {
            return Add(configurationSource, load: true);
        }
        public Setting Add(JsonSettingsSource configurationSource, bool load)
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(string key, out string value)
        {
            throw new NotImplementedException();
        }
    }
}
