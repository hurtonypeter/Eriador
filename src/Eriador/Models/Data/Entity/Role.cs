﻿using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Eriador.Models.Data.Entity
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<RolePermission> Permissions { get; } = new List<RolePermission>();

    }
}