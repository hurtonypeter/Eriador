using System;
using System.Collections.Generic;

namespace Eriador.Framework.Schemas
{
    public class ModuleSchema
    {
        public string ModuleId { get; set; }

        public string Name { get; set; }

        public List<PermissionSchema> Permissions { get; set; }

		public List<MenuSchema> Menus { get; set; }
	}
}