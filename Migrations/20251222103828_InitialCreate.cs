using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RVBARBER.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Closures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MorningOpen = table.Column<TimeSpan>(type: "time", nullable: true),
                    MorningClose = table.Column<TimeSpan>(type: "time", nullable: true),
                    AfternoonOpen = table.Column<TimeSpan>(type: "time", nullable: true),
                    AfternoonClose = table.Column<TimeSpan>(type: "time", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Closures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpeningHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    MorningOpen = table.Column<TimeSpan>(type: "time", nullable: false),
                    MorningClose = table.Column<TimeSpan>(type: "time", nullable: false),
                    AfternoonOpen = table.Column<TimeSpan>(type: "time", nullable: false),
                    AfternoonClose = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeTrattamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prezzo = table.Column<double>(type: "float", nullable: false),
                    DurataMinuti = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataPrenotazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAppuntamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Totale = table.Column<double>(type: "float", nullable: false),
                    TrattamentiJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durata = table.Column<double>(type: "float", nullable: false),
                    Cliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Barbiere = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OpeningHours",
                columns: new[] { "Id", "AfternoonClose", "AfternoonOpen", "DayOfWeek", "IsClosed", "MorningClose", "MorningOpen" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0), 0, true, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) },
                    { 2, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0), 1, true, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) },
                    { 3, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 14, 0, 0, 0), 2, false, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) },
                    { 4, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0), 3, false, new TimeSpan(0, 21, 0, 0, 0), new TimeSpan(0, 13, 0, 0, 0) },
                    { 5, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 14, 0, 0, 0), 4, false, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) },
                    { 6, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 14, 0, 0, 0), 5, false, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) },
                    { 7, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0), 6, false, new TimeSpan(0, 16, 0, 0, 0), new TimeSpan(0, 10, 0, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Closures");

            migrationBuilder.DropTable(
                name: "OpeningHours");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
