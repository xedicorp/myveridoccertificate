using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Dal.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemplate",
                table: "RegisterUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTemplate",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTemplate",
                table: "RegisterUsers");

            migrationBuilder.DropColumn(
                name: "IsTemplate",
                table: "AppUsers");
        }
    }
}
