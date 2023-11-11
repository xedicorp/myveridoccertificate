using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Dal.Migrations
{
    public partial class USG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "TransectionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "TransectionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "TransectionHistories");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "TransectionHistories");
        }
    }
}
