using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class ChangeUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IntroducerUsers_UserRole_UserRoleId",
                schema: "dbo",
                table: "IntroducerUsers");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "dbo");

            migrationBuilder.AddForeignKey(
                name: "FK_IntroducerUsers_IntroducerUserRole_UserRoleId",
                schema: "dbo",
                table: "IntroducerUsers",
                column: "UserRoleId",
                principalSchema: "dbo",
                principalTable: "IntroducerUserRole",
                principalColumn: "UserRoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IntroducerUsers_IntroducerUserRole_UserRoleId",
                schema: "dbo",
                table: "IntroducerUsers");

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "dbo",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_IntroducerUsers_UserRole_UserRoleId",
                schema: "dbo",
                table: "IntroducerUsers",
                column: "UserRoleId",
                principalSchema: "dbo",
                principalTable: "UserRole",
                principalColumn: "UserRoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
