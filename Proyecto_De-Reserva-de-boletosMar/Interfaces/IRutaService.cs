
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IRutaService
    {
        Task<IEnumerable<Ruta>> ObtenerTodasAsync();

        Task<Ruta?> ObtenerPorIdAsync(int id);

        Task<bool> RegistrarAsync(Ruta ruta);

        Task<bool> ActualizarAsync(int id, Ruta ruta);

        Task<bool> CambiarEstadoAsync(int id);
    }
}
