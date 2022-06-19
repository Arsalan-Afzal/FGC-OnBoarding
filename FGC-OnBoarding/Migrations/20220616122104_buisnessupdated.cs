using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class buisnessupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RiskIndicator",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskIndicator",
                schema: "dbo",
                table: "BuisnessProfile");
        }
    }
}
