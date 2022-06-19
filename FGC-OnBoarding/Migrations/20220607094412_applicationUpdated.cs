using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class applicationUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentStatus",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompliance",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsComplianceCommitee",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnboarded",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentStatus",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "IsCompliance",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "IsComplianceCommitee",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "IsOnboarded",
                schema: "dbo",
                table: "BuisnessProfile");
        }
    }
}
