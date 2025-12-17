using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RVBARBER.Migrations
{
    /// <inheritdoc />
    public partial class RefactorClosureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Closures",
                newName: "MorningOpen");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Closures",
                newName: "MorningClose");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AfternoonClose",
                table: "Closures",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AfternoonOpen",
                table: "Closures",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Closures",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfternoonClose",
                table: "Closures");

            migrationBuilder.DropColumn(
                name: "AfternoonOpen",
                table: "Closures");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Closures");

            migrationBuilder.RenameColumn(
                name: "MorningOpen",
                table: "Closures",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "MorningClose",
                table: "Closures",
                newName: "EndDate");
        }
    }
}
