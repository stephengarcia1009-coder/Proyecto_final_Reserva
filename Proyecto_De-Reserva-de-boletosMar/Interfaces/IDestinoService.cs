
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IDestinoService
    {
        Task<IEnumerable<Destino>> ObtenerTodosAsync();

        Task<Destino?> ObtenerPorIdAsync(int id);

        Task<bool> RegistrarAsync(Destino destino);

        Task<bool> ActualizarAsync(int id, Destino destino);

        Task<bool> CambiarEstadoAsync(int id);
    }
}
