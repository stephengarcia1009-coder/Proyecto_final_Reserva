
using System.ComponentModel.DataAnnotations;

namespace Proyecto_De_Reserva_de_boletosMar.ViewModels
{
    public class ReservaPresencialViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un cliente")]
        public int ClienteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un viaje")]
        public int ViajeId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debes registrar al menos un pasajero")]
        public List<PasajeroViewModel> Pasajeros { get; set; }
            = new List<PasajeroViewModel>();
    }
}
