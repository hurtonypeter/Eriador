using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eriador.Framework.Services.Settings
{
    public interface ISettingsService
    {
        string Get(string key);
        void Set(string key, string value);
    }
}
