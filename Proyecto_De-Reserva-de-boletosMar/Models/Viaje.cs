
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.Models
{
    public class Viaje
    {
        public int ViajeId { get; set; }

        [Required]
        public int RutaId { get; set; }

        [Required]
        public int PangaId { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        [Required]
        public int EstadoViajeId { get; set; }

        [StringLength(250)]
        public string? Observaciones { get; set; }

        public Ruta? Ruta { get; set; }

        public Panga? Panga { get; set; }

        public EstadoViaje? EstadoViaje { get; set; }
    }
}
