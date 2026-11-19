using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Proyecto_De_Reserva_de_boletosMar.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDatosIniciales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EstadosReserva",
                columns: new[] { "EstadoReservaId", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Reserva esperando el pago", "Pendiente de pago" },
                    { 2, "Reserva pagada y confirmada", "Confirmada" },
                    { 3, "Reserva cancelada", "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "EstadosViaje",
                columns: new[] { "EstadoViajeId", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Viaje pendiente de realizarse", "Programado" },
                    { 2, "Viaje cancelado", "Cancelado" },
                    { 3, "Viaje realizado", "Finalizado" }
                });

            migrationBuilder.InsertData(
                table: "MetodosPago",
                columns: new[] { "MetodoPagoId", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Pago presencial", "Efectivo" },
                    { 2, "Pago realizado mediante simulación", "Pago simulado" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EstadosReserva",
                keyColumn: "EstadoReservaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstadosReserva",
                keyColumn: "EstadoReservaId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstadosReserva",
                keyColumn: "EstadoReservaId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EstadosViaje",
                keyColumn: "EstadoViajeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstadosViaje",
                keyColumn: "EstadoViajeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstadosViaje",
                keyColumn: "EstadoViajeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MetodosPago",
                keyColumn: "MetodoPagoId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MetodosPago",
                keyColumn: "MetodoPagoId",
                keyValue: 2);
        }
    }
}
