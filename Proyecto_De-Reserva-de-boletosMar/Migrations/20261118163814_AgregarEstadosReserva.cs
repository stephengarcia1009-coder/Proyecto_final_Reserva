using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_De_Reserva_de_boletosMar.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEstadosReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosReserva",
                columns: table => new
                {
                    EstadoReservaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosReserva", x => x.EstadoReservaId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadosReserva");
        }
    }
}
