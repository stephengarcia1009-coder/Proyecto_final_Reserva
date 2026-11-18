using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_De_Reserva_de_boletosMar.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRutas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rutas",
                columns: table => new
                {
                    RutaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrigenId = table.Column<int>(type: "int", nullable: false),
                    DestinoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activa = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutas", x => x.RutaId);
                    table.ForeignKey(
                        name: "FK_Rutas_Destinos_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "Destinos",
                        principalColumn: "DestinoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rutas_Destinos_OrigenId",
                        column: x => x.OrigenId,
                        principalTable: "Destinos",
                        principalColumn: "DestinoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_DestinoId",
                table: "Rutas",
                column: "DestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_OrigenId",
                table: "Rutas",
                column: "OrigenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rutas");
        }
    }
}
