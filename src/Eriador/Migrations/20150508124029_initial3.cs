using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace Eriador.Migrations
{
    public partial class initial3 : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.AddColumn(
                name: "Status",
                table: "NewsPaper",
                type: "int",
                nullable: false);
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropColumn(name: "Status", table: "NewsPaper");
        }
    }
}
