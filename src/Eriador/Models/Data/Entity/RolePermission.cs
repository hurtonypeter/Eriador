using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eriador.Models.Data.Entity
{
    public class RolePermission
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
