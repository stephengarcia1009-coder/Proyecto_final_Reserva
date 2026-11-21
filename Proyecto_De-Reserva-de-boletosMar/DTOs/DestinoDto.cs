
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.DTOs
{
    public class DestinoDto
    {
        public int DestinoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;
    }
}
