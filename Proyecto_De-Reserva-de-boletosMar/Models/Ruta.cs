
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.Models
{
    public class Ruta
    {
        public int RutaId { get; set; }

        [Required]
        public int OrigenId { get; set; }

        [Required]
        public int DestinoId { get; set; }

        [StringLength(250)]
        public string? Descripcion { get; set; }

        public bool Activa { get; set; } = true;

        public Destino? Origen { get; set; }

        public Destino? Destino { get; set; }
    }
}
