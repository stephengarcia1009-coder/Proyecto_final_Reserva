
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.Models
{
    public class Pago
    {
        public int PagoId { get; set; }

        [Required]
        public int ReservaId { get; set; }

        [Required]
        public int MetodoPagoId { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Range(0.01, 10000000)]
        public decimal Monto { get; set; }

        [Required]
        [StringLength(30)]
        public string Estado { get; set; } = "Pendiente";

        public Reserva? Reserva { get; set; }

        public MetodoPago? MetodoPago { get; set; }
    }
}
