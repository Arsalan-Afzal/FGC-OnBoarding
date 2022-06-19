using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class CommentsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuisnessProfileID",
                schema: "dbo",
                table: "ExternalAttachmentComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExternalSearchId",
                schema: "dbo",
                table: "ExternalAttachmentComments",
                nullable: false,
                defaultValue: 0);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuisnessProfileID",
                schema: "dbo",
                table: "ExternalAttachmentComments");

            migrationBuilder.DropColumn(
                name: "ExternalSearchId",
                schema: "dbo",
                table: "ExternalAttachmentComments");

        }
    }
}
