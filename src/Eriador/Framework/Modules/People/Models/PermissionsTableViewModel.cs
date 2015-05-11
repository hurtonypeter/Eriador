using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Models.Data.Entity;

namespace Eriador.Framework.Modules.People.Models
{
    public class PermissionsTableViewModel
    {
        public List<Role> Roles { get; set; }

        public List<ModuleWithPermissions> Modules { get; set; } = new List<ModuleWithPermissions>();

    }

    public class ModuleWithPermissions
    {
        public string ModuleName { get; set; }

        public List<PermissionLine> Permissions { get; set; } = new List<PermissionLine>();
    }

    public class PermissionLine
    {
        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public List<bool> Roles { get; set; } = new List<bool>();
    }
}
