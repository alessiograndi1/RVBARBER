using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RVBARBER.Migrations
{
    /// <inheritdoc />
    public partial class AddDescrizioneTrattamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descrizione",
                table: "Treatments",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descrizione",
                table: "Treatments");
        }
    }
}
