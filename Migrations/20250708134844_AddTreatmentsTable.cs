using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RVBARBER.Migrations
{
    /// <inheritdoc />
    public partial class AddTreatmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomeTrattamento = table.Column<string>(type: "TEXT", nullable: false),
                    Prezzo = table.Column<decimal>(type: "TEXT", nullable: false),
                    DurataMinuti = table.Column<int>(type: "INTEGER", nullable: false),
                    Categoria = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Treatments");
        }
    }
}
