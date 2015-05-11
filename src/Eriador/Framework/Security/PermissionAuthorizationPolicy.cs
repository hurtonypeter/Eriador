
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Authorization;

namespace Eriador.Framework.Security
{
    public class PermissionAuthorizationPolicy : AuthorizationPolicy
    {

        public PermissionAuthorizationPolicy(IEnumerable<IAuthorizationRequirement> requirements, 
            IEnumerable<string> activeAuthenticationSchemes) : base(requirements, activeAuthenticationSchemes)
        {
            
        }

        public new static AuthorizationPolicy Combine( params AuthorizationPolicy[] policies)
        {
            return Combine((IEnumerable<AuthorizationPolicy>)policies);
        }

        // TODO: Add unit tests
        public new static AuthorizationPolicy Combine( IEnumerable<AuthorizationPolicy> policies)
        {
            var builder = new AuthorizationPolicyBuilder();
            foreach (var policy in policies)
            {
                builder.Combine(policy);
            }
            return builder.Build();
        }

        public new static AuthorizationPolicy Combine( AuthorizationOptions options, IEnumerable<AuthorizeAttribute> attributes)
        {
            var policyBuilder = new AuthorizationPolicyBuilder();
            bool any = false;
            foreach (var authorizeAttribute in attributes.OfType<AuthorizeAttribute>())
            {
                any = true;
                var requireAnyAuthenticated = true;
                if (!string.IsNullOrWhiteSpace(authorizeAttribute.Policy))
                {
                    var policy = options.GetPolicy(authorizeAttribute.Policy);
                    if (policy == null)
                    {
                        throw new InvalidOperationException("anyád");
                    }
                    policyBuilder.Combine(policy);
                    requireAnyAuthenticated = false;
                }
                var rolesSplit = authorizeAttribute.Roles?.Split(',');
                if (rolesSplit != null && rolesSplit.Any())
                {
                    policyBuilder.RequireRole(rolesSplit);
                    requireAnyAuthenticated = false;
                }
                string[] authTypesSplit = authorizeAttribute.ActiveAuthenticationSchemes?.Split(',');
                if (authTypesSplit != null && authTypesSplit.Any())
                {
                    foreach (var authType in authTypesSplit)
                    {
                        policyBuilder.ActiveAuthenticationSchemes.Add(authType);
                    }
                }
                if (requireAnyAuthenticated)
                {
                    policyBuilder.RequireAuthenticatedUser();
                }
            }
            return any ? policyBuilder.Build() : null;
        }
    }
}
