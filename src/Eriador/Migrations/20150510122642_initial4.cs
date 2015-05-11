using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace Eriador.Migrations
{
    public partial class initial4 : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.AlterColumn(
                name: "Sent",
                table: "NewsPaper",
                type: "datetime2",
                nullable: true);
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.AlterColumn(
                name: "Sent",
                table: "NewsPaper",
                type: "datetime2",
                nullable: false);
        }
    }
}
