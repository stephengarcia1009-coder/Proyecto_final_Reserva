
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.Models
{
    public class Tarifa
    {
        public int TarifaId { get; set; }

        [Required]
        public int RutaId { get; set; }

        [Range(0.01, 1000000)]
        public decimal Precio { get; set; }

        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        public Ruta? Ruta { get; set; }
    }
}
