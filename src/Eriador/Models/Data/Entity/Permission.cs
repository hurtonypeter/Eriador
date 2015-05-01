using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eriador.Models.Data.Entity
{
    public class Permission
    {
        public int Id { get; set; }

        [Required]
        public string MachineReadableName { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual Module Module { get; set; }

    }
}