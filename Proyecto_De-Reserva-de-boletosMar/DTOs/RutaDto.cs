
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.DTOs
{
    public class RutaDto
    {
        public int RutaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El origen es obligatorio.")]
        public int OrigenId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El destino es obligatorio.")]
        public int DestinoId { get; set; }

        [StringLength(250)]
        public string? Descripcion { get; set; }

        public bool Activa { get; set; } = true;
    }
}
