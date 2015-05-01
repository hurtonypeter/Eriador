using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Eriador.Models.Data.Entity
{
    public class Module
    {
        public int Id { get; set; }

        [Required]
        public string MachineReadableName { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}