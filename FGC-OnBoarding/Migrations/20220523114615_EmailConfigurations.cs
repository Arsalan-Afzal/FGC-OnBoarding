using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class EmailConfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                schema: "dbo",
                table: "IntroducersUsersApplications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "EmailEvents",
                schema: "dbo",
                columns: table => new
                {
                    EmailEventsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailEvents", x => x.EmailEventsId);
                });

            migrationBuilder.CreateTable(
                name: "EventsEmails",
                schema: "dbo",
                columns: table => new
                {
                    EventsEmailsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsEmails", x => x.EventsEmailsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailEvents",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EventsEmails",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                schema: "dbo",
                table: "IntroducersUsersApplications");
        }
    }
}
