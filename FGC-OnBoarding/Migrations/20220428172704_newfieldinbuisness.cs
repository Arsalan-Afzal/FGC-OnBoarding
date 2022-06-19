using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class newfieldinbuisness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ispep",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Peprelationship",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ispep",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "Peprelationship",
                schema: "dbo",
                table: "BuisnessProfile");
        }
    }
}
