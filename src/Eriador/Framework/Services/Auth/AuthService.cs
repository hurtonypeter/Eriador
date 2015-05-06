using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Eriador.Models.Data;
using Eriador.Models.Data.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using Microsoft.Framework.Caching.Memory;

namespace Eriador.Framework.Services.Auth
{
    /// <summary>
    /// Aktuálisan bejelentkezett felhasználó tulajdonságaihoz fér hozzá
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Az aktuális Http kontextus
        /// </summary>
        private readonly HttpContext HttpContext;

        /// <summary>
        /// EF DbContext osztálya az alkalmazás tábláival
        /// </summary>
        private readonly ApplicationDbContext db;

        /// <summary>
        /// AspNet szintű globális memória-cache
        /// </summary>
        private readonly IMemoryCache Cache;
        
        /// <summary>
        /// DI konstruktor
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <param name="context"></param>
        /// <param name="cache"></param>
        public AuthService(IHttpContextAccessor contextAccessor,
            ApplicationDbContext context,
            IMemoryCache cache)
        {
            HttpContext = contextAccessor.HttpContext;
            db = context;
            Cache = cache;
        }

        /// <summary>
        /// Az aktuális felhasználó azonosítóját adja vissza, 
        /// ha nincs bejelentkezve senki, akkor az anonymous felhasználóét.
        /// </summary>
        /// <returns></returns>
        public int CurrentUserId()
        {
            if (IsAuthenticated())
            {
                int tmp;
                return Int32.TryParse(HttpContext.User.GetUserId(), out tmp) ? tmp : default(int);
            }
            else
            {
                return db.Users.Single(u => u.UserName == "anonymous").Id;
            }
        }

        /// <summary>
        /// Megmondja, hogy van-e bejelentkezve felhasználó.
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Visszaadja az aktuális felhasználó(bejelentkezett vagy anonymous)
        /// által birtokolt jogosultságokat.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Permission> CurrentPermissions()
        {
            //List<Permission> permissions = Cache.Get("currentpermissions:" + CurrentUserId()) as List<Permission>;
            //if (permissions == null)
            //{
                var raw = @"select [Permission].[Id], [Permission].[MachineReadableName], [Permission].[ModuleId], [Permission].[Name] from [Permission] join [AspNetRolePermissions] on [Permission].[Id] = [AspNetRolePermissions].[PermissionId] join [AspNetRoles] on [AspNetRolePermissions].[RoleId] = [AspNetRoles].[Id] join [AspNetUserRoles] on [AspNetRoles].[Id] = [AspNetUserRoles].[RoleId] join [AspNetUsers] on [AspNetUserRoles].[UserId] = [AspNetUsers].[Id] where [AspNetUsers].[Id] = ";
                raw += CurrentUserId();
                var permissions = db.Permissions.FromSql(raw).ToList();

            //    Cache.Set("currentpermissions:" + CurrentUserId(), permissions);
            //}

            return permissions;
        }

        /// <summary>
        /// Visszaad az aktuális felhasználó(bejelentkezett vagy anonymous)
        /// által birtokolt jogosultságok azonosítóit(MachineReadableName-eket)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> CurrentPermissionNames()
        {
            return CurrentPermissions().Select(p => p.MachineReadableName);
        }

        /// <summary>
        /// Megmondja, hogy az aktuális felhasználó(bejelentkezett vagy anonymous)
        /// rendelkezik-e a paraméterül adott jogosultsággal
        /// </summary>
        /// <param name="perm">A vizsgálandó jogosultság MachineReadableName-je</param>
        /// <returns></returns>
        public bool HasPermission(string perm)
        {
            return CurrentPermissionNames().Any(p => p.ToLower() == perm.ToLower());
        }
        

        public static bool HasPermission(string perm, ClaimsPrincipal user)
        {
            using (var db = new ApplicationDbContext())
            {
                int currentUserId;
                if (user.Identity.IsAuthenticated)
                {
                    int tmp;
                    currentUserId = Int32.TryParse(user.GetUserId(), out tmp) ? tmp : default(int);
                }
                else
                {
                    currentUserId = db.Users.Single(u => u.UserName == "anonymous").Id;
                }

                var raw = @"select [Permission].[Id], [Permission].[MachineReadableName], [Permission].[ModuleId], [Permission].[Name] from [Permission] join [AspNetRolePermissions] on [Permission].[Id] = [AspNetRolePermissions].[PermissionId] join [AspNetRoles] on [AspNetRolePermissions].[RoleId] = [AspNetRoles].[Id] join [AspNetUserRoles] on [AspNetRoles].[Id] = [AspNetUserRoles].[RoleId] join [AspNetUsers] on [AspNetUserRoles].[UserId] = [AspNetUsers].[Id] where [AspNetUsers].[Id] = ";
                raw += currentUserId;
                var permissions = db.Permissions.FromSql(raw).ToList().Select(p => p.MachineReadableName);

                return permissions.Any(p => p.ToLower() == perm.ToLower());
            }
        }
    }
}
