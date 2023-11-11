using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Dal.Migrations
{
    public partial class PlanInfo_template : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TemplatePrice",
                table: "PlanInfos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplatePrice",
                table: "PlanInfos");
        }
    }
}
