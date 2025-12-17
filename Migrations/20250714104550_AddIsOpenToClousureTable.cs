using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RVBARBER.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOpenToClousureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "Closures",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "Closures");
        }
    }
}
