using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class CommentsUpdateaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionBy",
                schema: "dbo",
                table: "ExternalAttachmentComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionBy",
                schema: "dbo",
                table: "ExternalAttachmentComments");
        }
    }
}
