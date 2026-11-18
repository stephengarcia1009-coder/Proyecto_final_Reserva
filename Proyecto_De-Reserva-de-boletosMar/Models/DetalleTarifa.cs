
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.Models
{
    public class DetalleTarifa
    {
        public int DetalleTarifaId { get; set; }

        [Required]
        public int TarifaId { get; set; }

        [Range(0.01, 1000000)]
        public decimal PrecioAnterior { get; set; }

        [Range(0.01, 1000000)]
        public decimal PrecioNuevo { get; set; }

        public DateTime FechaCambio { get; set; } = DateTime.Now;

        public Tarifa? Tarifa { get; set; }
    }
}
