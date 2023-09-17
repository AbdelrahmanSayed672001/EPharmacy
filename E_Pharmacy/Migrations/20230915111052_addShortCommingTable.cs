using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Pharmacy.Migrations
{
    public partial class addShortCommingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortCommings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medicineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortCommings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortCommings_Medicines_medicineId",
                        column: x => x.medicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortCommings_medicineId",
                table: "ShortCommings",
                column: "medicineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortCommings");
        }
    }
}
