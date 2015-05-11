using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Models.Data.Entity;
using Eriador.Modules.News.Models.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using Microsoft.Data.Entity.SqlServer.Design.ReverseEngineering.Model;

namespace Eriador.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<RolePermission> RolePermission { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<NewsItem> NewsItems { get; set; }

        public DbSet<NewsPaper> NewsPapers { get; set; }

        private static bool _created;

        public ApplicationDbContext()
        {
            // Create the database and schema if it doesn't exist
            if (!_created)
            {
                Database.AsRelational().ApplyMigrations();
                _created = true;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=aspnet5-Eriador-2fc2e9fa-83dd-4cf6-a997-f45113287af9;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Permission>(b =>
            {
                b.Key(p => p.Id);
                b.Collection(p => p.Roles).InverseReference().ForeignKey(rp => rp.PermissionId);
            });
            builder.Entity<RolePermission>(b =>
            {
                b.Key(r => new { r.RoleId, r.PermissionId });
            });
            builder.Entity<Role>(b =>
            {
                b.Collection(r => r.Permissions).InverseReference().ForeignKey(rp => rp.RoleId);
            });
        }
    }
}
