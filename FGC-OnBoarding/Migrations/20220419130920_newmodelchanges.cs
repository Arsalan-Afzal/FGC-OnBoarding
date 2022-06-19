using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class newmodelchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntroducersUsersApplications",
                schema: "dbo",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuisnessProfileId = table.Column<int>(nullable: false),
                    BuisnessName = table.Column<string>(nullable: true),
                    BuisnessType = table.Column<string>(nullable: true),
                    SubmitDate = table.Column<DateTime>(nullable: false),
                    DeclinedDate = table.Column<DateTime>(nullable: false),
                    DeclinedReason = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    DeleteDate = table.Column<DateTime>(nullable: false),
                    IntroducerUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntroducersUsersApplications", x => x.ApplicationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntroducersUsersApplications",
                schema: "dbo");
        }
    }
}
