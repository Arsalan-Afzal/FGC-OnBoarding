using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class UpdateAllModelsIsDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "UserRole",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "UserLogs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "Trustees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "SoleDocuments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "ProfileComments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "PersonalDocuments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "OwnerShip",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "IntroducerUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "IntroducerUserRole",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "Introducers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "DirectorAndShareHolders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "CustomerLogs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "Currency",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "CharityDocument",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessSector",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessInformation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessDocuments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessAttachemtns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "dbo",
                table: "AuthorizedRepresentative",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "UserLogs");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "Trustees");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "SoleDocuments");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "ProfileComments");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "PersonalDocuments");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "OwnerShip");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "IntroducerUsers");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "IntroducerUserRole");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "Introducers");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "DirectorAndShareHolders");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "CustomerLogs");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "CharityDocument");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessTypes");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessSector");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessInformation");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessDocuments");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "BuisnessAttachemtns");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "dbo",
                table: "AuthorizedRepresentative");
        }
    }
}
