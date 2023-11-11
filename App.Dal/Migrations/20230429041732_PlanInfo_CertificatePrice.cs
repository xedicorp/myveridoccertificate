using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Dal.Migrations
{
    public partial class PlanInfo_CertificatePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CertificatesPrice",
                table: "PlanInfos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificatesPrice",
                table: "PlanInfos");
        }
    }
}
