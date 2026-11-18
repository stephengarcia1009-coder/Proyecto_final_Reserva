
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.Models
{
    public class DetalleReserva
    {
        public int DetalleReservaId { get; set; }

        [Required]
        public int ReservaId { get; set; }

        [Required]
        [StringLength(150)]
        public string NombrePasajero { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string TelefonoPasajero { get; set; } = string.Empty;

        [Range(0.01, 1000000)]
        public decimal PrecioUnitario { get; set; }

        public Reserva? Reserva { get; set; }
    }
}
