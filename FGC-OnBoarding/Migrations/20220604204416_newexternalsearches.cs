using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class newexternalsearches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalSearches",
                schema: "dbo",
                columns: table => new
                {
                    ExternalsearchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalSearches", x => x.ExternalsearchId);
                });

            migrationBuilder.CreateTable(
                name: "ExternalSearchesAttachments",
                schema: "dbo",
                columns: table => new
                {
                    AttachementsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ExternalsearchId = table.Column<int>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalSearchesAttachments", x => x.AttachementsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalSearches",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ExternalSearchesAttachments",
                schema: "dbo");
        }
    }
}
