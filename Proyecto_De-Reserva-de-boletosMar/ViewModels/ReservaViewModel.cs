
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.ViewModels
{
    public class ReservaViewModel
    {
        [Required]
        public int ViajeId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debes registrar al menos un pasajero")]
        public List<PasajeroViewModel> Pasajeros { get; set; }
            = new List<PasajeroViewModel>();
    }

    public class PasajeroViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string NombrePasajero { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20)]
        public string TelefonoPasajero { get; set; } = string.Empty;
    }
}
