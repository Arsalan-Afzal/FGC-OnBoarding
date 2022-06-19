using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class sendbackreason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SendBackReason",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendBackReason",
                schema: "dbo",
                table: "BuisnessProfile");
        }
    }
}
