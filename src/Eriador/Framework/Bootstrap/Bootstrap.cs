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
            //sémavalidáció vmiért elszáll, mintha nem lennének kompatibilisek a libek
            //var moduleSchema = schemagenerator.Generate(typeof(ModuleSchema));
            db = new ApplicationDbContext();
            
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\Pet\Documents\Visual Studio 2015\Projects\Eriador\src\Eriador\Modules");
            foreach (var moduleFolder in di.GetDirectories())
            {
                foreach (var file in moduleFolder.GetFiles().Where(f => f.Name.EndsWith(".info.json")))
                {
                    
                    //JObject  obj = JObject.Parse(File.ReadAllText(file.FullName));
                    //if(obj.IsValid(moduleSchema))
                    //{

                    //}
                    var module = JsonConvert.DeserializeObject<ModuleSchema>(File.ReadAllText(file.FullName));
                    InitModule(module);
                        

                }
                
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
                m = new Models.Data.Entity.Module
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
                    p = new Models.Data.Entity.Permission
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
                    menu = new Models.Data.Entity.MenuItem
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