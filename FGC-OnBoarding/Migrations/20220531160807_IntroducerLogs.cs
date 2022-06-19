using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class IntroducerLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntroducerLogs",
                schema: "dbo",
                columns: table => new
                {
                    IntrouducersLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntroducerName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IPAdress = table.Column<string>(nullable: true),
                    LoginTime = table.Column<DateTime>(nullable: true),
                    LoginTimeStr = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    FormName = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    FieldName = table.Column<string>(nullable: true),
                    CountryName = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    LogOutTime = table.Column<DateTime>(nullable: true),
                    ActionTime = table.Column<DateTime>(nullable: true),
                    LogOutTimeStr = table.Column<string>(nullable: true),
                    Activity = table.Column<string>(nullable: true),
                    IntroducerId = table.Column<string>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntroducerLogs", x => x.IntrouducersLogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntroducerLogs",
                schema: "dbo");
        }
    }
}
