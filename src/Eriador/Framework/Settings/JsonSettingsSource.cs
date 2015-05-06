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

        public void Save(Stream stream)
        {
            JsonSettingsFileWriter writer = new JsonSettingsFileWriter();
            writer.Write(stream, Data);
        }
    }
}
