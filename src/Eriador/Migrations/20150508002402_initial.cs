using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace Eriador.Migrations
{
    public partial class initial : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "Module",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    IsActive = table.Column(type: "bit", nullable: false),
                    MachineReadableName = table.Column(type: "nvarchar(max)", nullable: true),
                    Name = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.Id);
                });
            migration.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    ConcurrencyStamp = table.Column(type: "nvarchar(max)", nullable: true),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    Name = table.Column(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });
            migration.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    AccessFailedCount = table.Column(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column(type: "nvarchar(max)", nullable: true),
                    Email = table.Column(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column(type: "bit", nullable: false),
                    FullName = table.Column(type: "nvarchar(max)", nullable: true),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    LockoutEnabled = table.Column(type: "bit", nullable: false),
                    LockoutEnd = table.Column(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column(type: "bit", nullable: false),
                    SecurityStamp = table.Column(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column(type: "bit", nullable: false),
                    UserName = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });
            migration.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    MachineReadableName = table.Column(type: "nvarchar(max)", nullable: true),
                    ModuleId = table.Column(type: "int", nullable: true),
                    Name = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_Module_ModuleId",
                        columns: x => x.ModuleId,
                        referencedTable: "Module",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    ClaimType = table.Column(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column(type: "nvarchar(max)", nullable: true),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    RoleId = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        columns: x => x.RoleId,
                        referencedTable: "AspNetRoles",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "NewsPaper",
                columns: table => new
                {
                    Created = table.Column(type: "datetime2", nullable: false),
                    EditorId = table.Column(type: "int", nullable: true),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    LastEdited = table.Column(type: "datetime2", nullable: false),
                    REditor = table.Column(type: "nvarchar(max)", nullable: true),
                    RPublisher = table.Column(type: "nvarchar(max)", nullable: true),
                    Sent = table.Column(type: "datetime2", nullable: false),
                    Title = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsPaper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsPaper_AspNetUsers_EditorId",
                        columns: x => x.EditorId,
                        referencedTable: "AspNetUsers",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    ClaimType = table.Column(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column(type: "nvarchar(max)", nullable: true),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    UserId = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        columns: x => x.UserId,
                        referencedTable: "AspNetUsers",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column(type: "nvarchar(450)", nullable: true),
                    ProviderDisplayName = table.Column(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        columns: x => x.UserId,
                        referencedTable: "AspNetUsers",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    RoleId = table.Column(type: "int", nullable: false),
                    UserId = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        columns: x => x.RoleId,
                        referencedTable: "AspNetRoles",
                        referencedColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        columns: x => x.UserId,
                        referencedTable: "AspNetUsers",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    MachineReadableName = table.Column(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column(type: "int", nullable: true),
                    PermissionId = table.Column(type: "int", nullable: true),
                    Route = table.Column(type: "nvarchar(max)", nullable: true),
                    Title = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItem_MenuItem_ParentId",
                        columns: x => x.ParentId,
                        referencedTable: "MenuItem",
                        referencedColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MenuItem_Permission_PermissionId",
                        columns: x => x.PermissionId,
                        referencedTable: "Permission",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    PermissionId = table.Column(type: "int", nullable: false),
                    RoleId = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        columns: x => x.PermissionId,
                        referencedTable: "Permission",
                        referencedColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolePermission_AspNetRoles_RoleId",
                        columns: x => x.RoleId,
                        referencedTable: "AspNetRoles",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "NewsItem",
                columns: table => new
                {
                    Body = table.Column(type: "nvarchar(max)", nullable: true),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    Link = table.Column(type: "nvarchar(max)", nullable: true),
                    NewsPaperId = table.Column(type: "int", nullable: true),
                    Title = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsItem_NewsPaper_NewsPaperId",
                        columns: x => x.NewsPaperId,
                        referencedTable: "NewsPaper",
                        referencedColumn: "Id");
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("MenuItem");
            migration.DropTable("Module");
            migration.DropTable("Permission");
            migration.DropTable("AspNetRoles");
            migration.DropTable("RolePermission");
            migration.DropTable("AspNetUsers");
            migration.DropTable("NewsItem");
            migration.DropTable("NewsPaper");
            migration.DropTable("AspNetRoleClaims");
            migration.DropTable("AspNetUserClaims");
            migration.DropTable("AspNetUserLogins");
            migration.DropTable("AspNetUserRoles");
        }
    }
}
