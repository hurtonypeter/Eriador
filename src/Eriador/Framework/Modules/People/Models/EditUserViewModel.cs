using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Models.Data.Entity;

namespace Eriador.Framework.Modules.People.Models
{
    public class EditUserViewModel
    {
        public User User { get; set; }

        public List<RoleCheckBox> UserRoles { get; set; } = new List<RoleCheckBox>();
    }

    public class RoleCheckBox
    {
        public string Name { get; set; }

        public bool Checked { get; set; }
    }
}
