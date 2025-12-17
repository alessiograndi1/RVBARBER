using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RVBARBER.Migrations
{
    /// <inheritdoc />
    public partial class AddOpeningHoursTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OpeningHours",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
