
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.Models
{
    public class Reserva
    {
        public int ReservaId { get; set; }

        [Required]
        [StringLength(30)]
        public string CodigoReserva { get; set; } = string.Empty;

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ViajeId { get; set; }

        [Required]
        public int EstadoReservaId { get; set; }

        public DateTime FechaReserva { get; set; } = DateTime.Now;

        [Range(1, 100)]
        public int CantidadBoletos { get; set; }

        [Range(0.01, 10000000)]
        public decimal Total { get; set; }

        public Cliente? Cliente { get; set; }

        public Viaje? Viaje { get; set; }

        public EstadoReserva? EstadoReserva { get; set; }
    }
}

