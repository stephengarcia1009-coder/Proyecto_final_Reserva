
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.DTOs
{
    public class TarifaDto
    {
        public int TarifaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una ruta.")]
        public int RutaId { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Precio { get; set; }

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}
