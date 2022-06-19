using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class newCustomerLogshhh11212asdasdas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerLogs",
                schema: "dbo",
                columns: table => new
                {
                    CustomerLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IPAdress = table.Column<string>(nullable: true),
                    LoginTime = table.Column<DateTime>(nullable: true),
                    LoginTimeStr = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    FormName = table.Column<string>(nullable: true),
                    FieldName = table.Column<string>(nullable: true),
                    CountryName = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    LogOutTime = table.Column<DateTime>(nullable: true),
                    LogOutTimeStr = table.Column<string>(nullable: true),
                    Activity = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLogs", x => x.CustomerLogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerLogs",
                schema: "dbo");
        }
    }
}
