using System;
using System.Collections.Generic;

namespace Eriador.Models.Data.Entity
{
    public class MenuItem
    {
		public int Id { get; set; }

        public string MachineReadableName { get; set; }

        public string Title { get; set; }

		public string Route { get; set; }

		public virtual MenuItem Parent { get; set; }

		public virtual ICollection<MenuItem> Children { get; set; }

		public virtual Permission Permission { get; set; }
	}
}