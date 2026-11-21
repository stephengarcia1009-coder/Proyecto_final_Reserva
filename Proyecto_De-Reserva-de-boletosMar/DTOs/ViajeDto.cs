using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.DTOs
{
    public class ViajeDto
    {
        public int ViajeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una ruta.")]
        public int RutaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una panga.")]
        public int PangaId { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        public DateTime FechaSalida { get; set; }

        [StringLength(250, ErrorMessage = "Las observaciones no pueden superar los 250 caracteres.")]
        public string? Observaciones { get; set; }
    }
}