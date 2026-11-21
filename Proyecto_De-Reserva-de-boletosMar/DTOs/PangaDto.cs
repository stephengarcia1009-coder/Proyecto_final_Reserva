
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.DTOs
{
    public class PangaDto
    {
        public int PangaId { get; set; }

        [Required(ErrorMessage = "El nombre de la panga es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "La capacidad debe ser mayor que cero")]
        public int Capacidad { get; set; }

        [StringLength(250)]
        public string? Descripcion { get; set; }
    }
}
