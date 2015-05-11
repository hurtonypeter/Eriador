using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;
using Eriador.Framework.Settings;

namespace Eriador.Framework.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        IConfiguration Config;

        public SettingsService()
        {
            //Config = new Configuration().AddJsonFile("config.json").GetSubKey("AppSettings");
            Config = new JsonSetting().Add(new JsonSettingsSource("settings.json"));
        }

        public string this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        public string Get(string key)
        {
            return Config[key];
        }

        public void Set(string key, string value)
        {
            Config[key] = value;
        }

        public static string GetStatic(string key)
        {
            var c = new JsonSetting().Add(new JsonSettingsSource("settings.json"));
            return c[key];
        }
    }
}
