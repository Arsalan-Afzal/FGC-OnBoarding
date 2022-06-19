using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class newCustomerLogshhh1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                schema: "dbo",
                table: "CustomerLogs",
                nullable: true);

         

            migrationBuilder.AddColumn<string>(
                name: "FormName",
                schema: "dbo",
                table: "CustomerLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewValue",
                schema: "dbo",
                table: "CustomerLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValue",
                schema: "dbo",
                table: "CustomerLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryName",
                schema: "dbo",
                table: "CustomerLogs");

           

            migrationBuilder.DropColumn(
                name: "FormName",
                schema: "dbo",
                table: "CustomerLogs");

            migrationBuilder.DropColumn(
                name: "NewValue",
                schema: "dbo",
                table: "CustomerLogs");

            migrationBuilder.DropColumn(
                name: "OldValue",
                schema: "dbo",
                table: "CustomerLogs");
        }
    }
}
