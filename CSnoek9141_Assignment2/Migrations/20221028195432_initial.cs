using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSnoek9141_Assignment2.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    PartyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.PartyID);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    InvitationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuestEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InviteStatus = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PartyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.InvitationID);
                    table.ForeignKey(
                        name: "FK_Invitations_Parties_PartyID",
                        column: x => x.PartyID,
                        principalTable: "Parties",
                        principalColumn: "PartyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "PartyID", "Description", "EventDate", "Location" },
                values: new object[] { 1, "New year's eve blast!", new DateTime(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Time Square, NY" });

            migrationBuilder.InsertData(
                table: "Invitations",
                columns: new[] { "InvitationID", "GuestEmail", "GuestName", "InviteStatus", "PartyID" },
                values: new object[] { 1, "pmadziak@conestogac.on.ca", "Bob Jones", "RespondedYes", 1 });

            migrationBuilder.InsertData(
                table: "Invitations",
                columns: new[] { "InvitationID", "GuestEmail", "GuestName", "InviteStatus", "PartyID" },
                values: new object[] { 2, "peter.madziak@gmail.com", "Sally Smith", "InviteSent", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_PartyID",
                table: "Invitations",
                column: "PartyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "Parties");
        }
    }
}
