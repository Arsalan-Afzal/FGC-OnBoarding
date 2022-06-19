using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class userlogsupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                schema: "dbo",
                table: "UserLogs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LoginTime",
                schema: "dbo",
                table: "UserLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                schema: "dbo",
                table: "UserLogs");

            migrationBuilder.DropColumn(
                name: "LoginTime",
                schema: "dbo",
                table: "UserLogs");
        }
    }
}
