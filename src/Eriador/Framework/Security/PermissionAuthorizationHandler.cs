using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;

namespace Eriador.Framework.Security
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirements>
    {
        public override void Handle(AuthorizationContext context, PermissionAuthorizationRequirements requirement)
        {
            bool found = false;
            if (context.User != null)
            {
                
            }
            else
            {

            }

            if (found)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
