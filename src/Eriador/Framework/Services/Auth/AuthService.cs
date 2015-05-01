using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Eriador.Models.Data;
using Eriador.Models.Data.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using Microsoft.Framework.Caching.Memory;

namespace Eriador.Framework.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> UserManager;
        private readonly HttpContext HttpContext;
        private readonly ApplicationDbContext db;
        private readonly IMemoryCache Cache;

        public AuthService(UserManager<User> userManager, 
            IHttpContextAccessor contextAccessor,
            ApplicationDbContext context,
            IMemoryCache cache)
        {
            UserManager = userManager;
            HttpContext = contextAccessor.HttpContext;
            db = context;
            Cache = cache;
        }

        public int CurrentUserId()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int tmp;
                return Int32.TryParse(HttpContext.User.GetUserId(), out tmp) ? tmp : default(int);
            }
            else
            {
                return db.Users.Single(u => u.UserName == "anonymous").Id;
            }
        }

        public bool IsAuthenticated()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }

        public IEnumerable<Permission> CurrentPermissions()
        {
            List<Permission> permissions = Cache.Get("currentpermissions:" + CurrentUserId()) as List<Permission>;
            if (permissions == null)
            {
                var raw = @"select [Permission].[Id], [Permission].[MachineReadableName], [Permission].[ModuleId], [Permission].[Name] from [Permission] join [AspNetRolePermissions] on [Permission].[Id] = [AspNetRolePermissions].[PermissionId] join [AspNetRoles] on [AspNetRolePermissions].[RoleId] = [AspNetRoles].[Id] join [AspNetUserRoles] on [AspNetRoles].[Id] = [AspNetUserRoles].[RoleId] join [AspNetUsers] on [AspNetUserRoles].[UserId] = [AspNetUsers].[Id] where [AspNetUsers].[Id] = ";
                raw += CurrentUserId();
                permissions = db.Permissions.FromSql(raw).ToList();
                Cache.Set("currentpermissions:" + CurrentUserId(), permissions);
            }

            return permissions;
        }

        public IEnumerable<string> CurrentPermissionNames()
        {
            return CurrentPermissions().Select(p => p.MachineReadableName);
        }

        public bool HasPermission(string perm)
        {
            return CurrentPermissionNames().Any(p => p.ToLower() == perm.ToLower());
        }
    }
}
