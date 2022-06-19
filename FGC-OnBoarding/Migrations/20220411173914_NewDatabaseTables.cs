using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class NewDatabaseTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "AutomatedKycrecieved",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutomatedKycsent",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclinedDate",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeclinedReason",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeclined",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OfferLetter",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OnboardingFee",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SignOfferLetter",
                schema: "dbo",
                table: "BuisnessProfile",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProfileComments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionBy = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    ActorsId = table.Column<int>(nullable: false),
                    ActionDate = table.Column<DateTime>(nullable: false),
                    BuisneesProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileComments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileComments",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "AutomatedKycrecieved",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "AutomatedKycsent",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "DeclinedDate",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "DeclinedReason",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "IsDeclined",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "OfferLetter",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "OnboardingFee",
                schema: "dbo",
                table: "BuisnessProfile");

            migrationBuilder.DropColumn(
                name: "SignOfferLetter",
                schema: "dbo",
                table: "BuisnessProfile");
        }
    }
}
