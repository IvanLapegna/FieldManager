using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservationsStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationsStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerUserID = table.Column<int>(type: "int", nullable: false),
                    ReservationStatusID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    MaxJugadores = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_Reservations_ReservationsStatus_ReservationStatusID",
                        column: x => x.ReservationStatusID,
                        principalTable: "ReservationsStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    ReservationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => new { x.ReservationID, x.UserID });
                    table.ForeignKey(
                        name: "FK_Players_Reservations_ReservationID",
                        column: x => x.ReservationID,
                        principalTable: "Reservations",
                        principalColumn: "ReservationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ReservationsStatus",
                columns: new[] { "Id", "Status" },
                values: new object[,]
                {
                    { 1, "Reserved" },
                    { 2, "Finished" },
                    { 3, "Cancelled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationStatusID",
                table: "Reservations",
                column: "ReservationStatusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "ReservationsStatus");
        }
    }
}
