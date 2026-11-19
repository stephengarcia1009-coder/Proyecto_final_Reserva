
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.Models
{
    public class DetalleViaje
    {
        public int DetalleViajeId { get; set; }

        [Required]
        public int ViajeId { get; set; }

        [Required]
        [StringLength(250)]
        public string DescripcionCambio { get; set; } = string.Empty;

        public DateTime FechaCambio { get; set; } = DateTime.Now;

        public Viaje? Viaje { get; set; }
    }
}
