using Eriador.Framework.Schemas;
using Eriador.Models.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Eriador.Framework.Bootstrap
{
    public class Bootstrap
    {
        private ApplicationDbContext db;
        public Bootstrap()
        {

        }

        public void Initialize()
        {
            InitializeModules();
        }

        private void InitializeModules()
        {
            //JSchemaGenerator schemagenerator = new JSchemaGenerator();
            //sémavalidáció vmiért elszáll, nem kompatibilisek a libek jelenleg
            //var moduleSchema = schemagenerator.Generate(typeof(ModuleSchema));
            db = new ApplicationDbContext();
            
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\Pet\Documents\Visual Studio 2015\Projects\Eriador\src\Eriador\Modules");
            foreach (var moduleFolder in di.GetDirectories())
            {
                foreach (var file in moduleFolder.GetFiles().Where(f => f.Name.EndsWith(".info.json")))
                {
                    //béta fázisú inkompatibilisség miatt kikommentezve a validáció
                    //JObject  obj = JObject.Parse(File.ReadAllText(file.FullName));
                    //if(obj.IsValid(moduleSchema))
                    //{

                    //}
                    var module = JsonConvert.DeserializeObject<ModuleSchema>(File.ReadAllText(file.FullName));
                    InitModule(module);
                        

                }
                
            }
            DirectoryInfo diFramework = new DirectoryInfo(@"C:\Users\Pet\Documents\Visual Studio 2015\Projects\Eriador\src\Eriador\Framework\Modules");
            foreach (var moduleFolder in diFramework.GetDirectories())
            {
                foreach (var file in moduleFolder.GetFiles().Where(f => f.Name.EndsWith(".info.json")))
                {
                    var module = JsonConvert.DeserializeObject<ModuleSchema>(File.ReadAllText(file.FullName));
                    InitModule(module);
                }

            }

            var aUser = db.Users.SingleOrDefault(u => u.UserName.ToLower() == "anonymous");
            if (aUser == null)
            {
                aUser = new Eriador.Models.Data.Entity.User
                {
                    UserName = "anonymous",
                    FullName = "Anonymous User",
                    SecurityStamp = "a957c65c-33a8-4dfc-9a31-f69859eedf9b",
                    ConcurrencyStamp = "910d9ec1-6790-4321-9095-cd133a16488a",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    EmailConfirmed = false
                };
                db.Users.Add(aUser);
            }
            var aRole = db.Roles.SingleOrDefault(u => u.Name.ToLower() == "anonymous");
            if (aRole == null)
            {
                aRole = new Eriador.Models.Data.Entity.Role
                {
                    Name = "Anonymous",
                    NormalizedName = "ANONYMOUS"
                };
                db.Roles.Add(aRole);
            }
            var authRole = db.Roles.SingleOrDefault(u => u.Name.ToLower() == "authenticated");
            if (authRole == null)
            {
                authRole = new Eriador.Models.Data.Entity.Role
                {
                    Name = "Authenticated",
                    NormalizedName = "AUTHENTICATED"
                };
                db.Roles.Add(authRole);
            }
            db.SaveChanges();

            var aUserRole = db.UserRoles.SingleOrDefault(u => u.UserId == aUser.Id && u.RoleId == aRole.Id);
            if (aUserRole == null)
            {
                aUserRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<int>
                {
                    UserId = aUser.Id,
                    RoleId = aRole.Id
                };
                db.UserRoles.Add(aUserRole);
            }

            db.SaveChanges();
            db.Dispose();
            //var modulesFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Modules/HKNews/hknews.info.json");
            //var infofile = @"C:\Users\Pet\Documents\Visual Studio 2015\Projects\Eriador\src\Eriador\Modules/HKNews/hknews.info.json";
            //var schema = JsonConvert.DeserializeObject<ModuleSchema>(File.ReadAllText(infofile));
        }

        private void InitModule(ModuleSchema module)
        {
            var m = db.Modules.SingleOrDefault(s => s.MachineReadableName.ToLower() == module.ModuleId.ToLower());
            if (m == null)
            {
                m = new Eriador.Models.Data.Entity.Module
                {
                    MachineReadableName = module.ModuleId,
                    Name = module.Name
                };
                db.Modules.Add(m);
            }

            foreach (var permission in module.Permissions)
            {
                var p = db.Permissions.SingleOrDefault(s => s.MachineReadableName.ToLower() == permission.PermissionId.ToLower());
                if (p == null)
                {
                    p = new Eriador.Models.Data.Entity.Permission
                    {
                        MachineReadableName = permission.PermissionId,
                        Name = permission.Name,
                        Module = m
                    };
                    db.Permissions.Add(p);
                }
            }
            db.SaveChanges();

            foreach (var item in module.Menus)
            {
                var menu = db.MenuItems.SingleOrDefault(s => s.MachineReadableName.ToLower() == item.MenuId.ToLower());
                if (menu == null)
                {
                    menu = new Eriador.Models.Data.Entity.MenuItem
                    {
                        MachineReadableName = item.MenuId,
                        Title = item.Title,
                        Route = item.Route,
                        Parent = string.IsNullOrWhiteSpace(item.ParentMenuId) ? null :
                            db.MenuItems.Single(s => s.MachineReadableName.ToLower() == item.ParentMenuId.ToLower()),
                        Permission = string.IsNullOrWhiteSpace(item.PermissionId) ? null :
                            db.Permissions.Single(p => p.MachineReadableName.ToLower() == item.PermissionId.ToLower())
                    };
                    db.MenuItems.Add(menu);
                    db.SaveChanges();
                }
            }
        }
    }
}