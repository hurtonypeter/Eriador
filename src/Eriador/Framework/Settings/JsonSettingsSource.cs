using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.ConfigurationModel;

namespace Eriador.Framework.Settings
{
    public class JsonSettingsSource : JsonConfigurationSource
    {
        public JsonSettingsSource(string path) : base(path)
        {
        }

        public JsonSettingsSource(string path, bool optional) : base(path, optional)
        {
        }

        public override void Load()
        {
            base.Load();
        }

        public override bool TryGet(string key, out string value)
        {
            return base.TryGet(key, out value);
        }

        public override void Set(string key, string value)
        {
            base.Set(key, value);
        }

        public void Save()
        {
            using (var stream = new FileStream(Path, FileMode.Truncate, FileAccess.Write))
            {
                JsonSettingsFileWriter writer = new JsonSettingsFileWriter();
                writer.Write(stream, Data); 
            }
        }
    }
}
