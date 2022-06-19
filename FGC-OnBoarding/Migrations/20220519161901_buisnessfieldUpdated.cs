using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class buisnessfieldUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                schema: "dbo",
                table: "CustomerLogs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActionTime",
                schema: "dbo",
                table: "CustomerLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                schema: "dbo",
                table: "CustomerLogs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClient",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuisnessName",
                schema: "dbo",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                schema: "dbo",
                table: "CustomerLogs");

            migrationBuilder.DropColumn(
                name: "ActionTime",
                schema: "dbo",
                table: "CustomerLogs");

            migrationBuilder.DropColumn(
                name: "Remarks",
                schema: "dbo",
                table: "CustomerLogs");

            migrationBuilder.DropColumn(
                name: "IsClient",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuisnessName",
                schema: "dbo",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
