using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;

namespace Eriador.Framework.Security
{
    public class PermissionAuthorizationRequirements : IAuthorizationRequirement
    {
        public string Permission { get; set; }
    }
}
