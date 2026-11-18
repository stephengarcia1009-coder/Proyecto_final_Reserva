
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.Models
{
    public class DetallePago
    {
        public int DetallePagoId { get; set; }

        [Required]
        public int PagoId { get; set; }

        public DateTime FechaIntento { get; set; } = DateTime.Now;

        [Range(0.01, 10000000)]
        public decimal Monto { get; set; }

        [Required]
        [StringLength(30)]
        public string Resultado { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Observacion { get; set; }

        public Pago? Pago { get; set; }
    }
}
