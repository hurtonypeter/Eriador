
using System.Collections.Generic;
using Microsoft.AspNet.Authorization;

namespace Eriador.Framework.Security
{
    public class PermissionAuthorizationPolicy : AuthorizationPolicy
    {

        public PermissionAuthorizationPolicy(IEnumerable<IAuthorizationRequirement> requirements, 
            IEnumerable<string> activeAuthenticationSchemes) : base(requirements, activeAuthenticationSchemes)
        {
        
        }

        
    }
}
