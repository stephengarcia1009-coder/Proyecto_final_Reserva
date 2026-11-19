
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObtenerTodosAsync();

        Task<Cliente?> ObtenerPorIdAsync(int id);

        Task<Cliente?> ObtenerPorUsuarioAsync(string usuarioId);

        Task<bool> ActualizarAsync(int id, Cliente cliente);
    }
}
