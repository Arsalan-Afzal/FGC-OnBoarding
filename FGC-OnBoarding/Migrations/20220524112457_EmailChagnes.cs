using Microsoft.EntityFrameworkCore.Migrations;

namespace FGC_OnBoarding.Migrations
{
    public partial class EmailChagnes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailEventsId",
                schema: "dbo",
                table: "EventsEmails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventsEmails_EmailEventsId",
                schema: "dbo",
                table: "EventsEmails",
                column: "EmailEventsId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventsEmails_EmailEvents_EmailEventsId",
                schema: "dbo",
                table: "EventsEmails",
                column: "EmailEventsId",
                principalSchema: "dbo",
                principalTable: "EmailEvents",
                principalColumn: "EmailEventsId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventsEmails_EmailEvents_EmailEventsId",
                schema: "dbo",
                table: "EventsEmails");

            migrationBuilder.DropIndex(
                name: "IX_EventsEmails_EmailEventsId",
                schema: "dbo",
                table: "EventsEmails");

            migrationBuilder.DropColumn(
                name: "EmailEventsId",
                schema: "dbo",
                table: "EventsEmails");
        }
    }
}
