
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IReservaService
    {
        Task<IEnumerable<Reserva>> ObtenerTodasAsync();

        Task<Reserva?> ObtenerPorIdAsync(int id);

        Task<IEnumerable<Reserva>> ObtenerPorClienteAsync(int clienteId);

        Task<bool> RegistrarAsync(Reserva reserva);

        Task<bool> CancelarAsync(int id);

        Task<bool> ConfirmarAsync(int id);
    }
}
