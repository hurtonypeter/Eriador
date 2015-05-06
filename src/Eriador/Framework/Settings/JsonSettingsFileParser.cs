using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Framework.ConfigurationModel.Json;

namespace Eriador.Framework.Settings
{
    public class JsonSettingsFileWriter
    {
        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new Stack<string>();
        private string _currentPath;

        private JsonTextWriter _writer;

        public void Write(Stream output, IDictionary<string,string> data)
        {
            _data.Clear();
            _writer = new JsonTextWriter(new StreamWriter(output));
            
            

        }



        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = string.Join(":", _context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = string.Join(":", _context.Reverse());
        }
    }
}
