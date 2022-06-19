using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class userlogsupdatedqq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                schema: "dbo",
                table: "UserLogs");

            migrationBuilder.AddColumn<string>(
                name: "CountryrName",
                schema: "dbo",
                table: "UserLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryrName",
                schema: "dbo",
                table: "UserLogs");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                schema: "dbo",
                table: "UserLogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
