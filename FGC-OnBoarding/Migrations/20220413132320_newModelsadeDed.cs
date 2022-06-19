using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class newModelsadeDed : Migration
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
                    LoginTime = table.Column<DateTime>(nullable: false),
                    LoginTimeStr = table.Column<string>(nullable: true),
                    LogOutTime = table.Column<DateTime>(nullable: false),
                    LogOutTimeStr = table.Column<string>(nullable: true),
                    Activity = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLogs", x => x.CustomerLogId);
                });

            migrationBuilder.CreateTable(
                name: "Introducers",
                schema: "dbo",
                columns: table => new
                {
                    IntroducerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntroducerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Introducers", x => x.IntroducerId);
                });

            migrationBuilder.CreateTable(
                name: "IntroducerUserRole",
                schema: "dbo",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntroducerUserRole", x => x.UserRoleId);
                });

            migrationBuilder.CreateTable(
                name: "UserLogs",
                schema: "dbo",
                columns: table => new
                {
                    LogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IPAdress = table.Column<string>(nullable: true),
                    LogDate = table.Column<DateTime>(nullable: false),
                    LogDateStr = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "dbo",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleId);
                });

            migrationBuilder.CreateTable(
                name: "IntroducerUsers",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IntroducersIntroducerId = table.Column<int>(nullable: true),
                    IntroducerId = table.Column<int>(nullable: false),
                    UserRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntroducerUsers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_IntroducerUsers_Introducers_IntroducersIntroducerId",
                        column: x => x.IntroducersIntroducerId,
                        principalSchema: "dbo",
                        principalTable: "Introducers",
                        principalColumn: "IntroducerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntroducerUsers_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalSchema: "dbo",
                        principalTable: "UserRole",
                        principalColumn: "UserRoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntroducerUsers_IntroducersIntroducerId",
                schema: "dbo",
                table: "IntroducerUsers",
                column: "IntroducersIntroducerId");

            migrationBuilder.CreateIndex(
                name: "IX_IntroducerUsers_UserRoleId",
                schema: "dbo",
                table: "IntroducerUsers",
                column: "UserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "IntroducerUserRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "IntroducerUsers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Introducers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "dbo");
        }
    }
}
