
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IViajeService
    {
        Task<IEnumerable<Viaje>> ObtenerTodosAsync();

        Task<Viaje?> ObtenerPorIdAsync(int id);

        Task<IEnumerable<Viaje>> ObtenerDisponiblesAsync(DateTime fecha);

        Task<bool> RegistrarAsync(Viaje viaje);

        Task<bool> ActualizarAsync(int id, Viaje viaje);

        Task<bool> CancelarAsync(int id);

       
    }
}
