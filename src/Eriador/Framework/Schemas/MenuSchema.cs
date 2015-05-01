using System;

namespace Eriador.Framework.Schemas
{
    public class MenuSchema
    {
		public string MenuId { get; set; }

		public string Title { get; set; }

        public string Route { get; set; }

        public string PermissionId { get; set; }

		public string ParentMenuId { get; set; }
	}
}