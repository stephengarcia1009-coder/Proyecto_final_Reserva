
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface ITarifaService
    {
        Task<IEnumerable<Tarifa>> ObtenerTodasAsync();

        Task<Tarifa?> ObtenerPorIdAsync(int id);

        Task<Tarifa?> ObtenerPorRutaAsync(int rutaId);

        Task<bool> RegistrarAsync(Tarifa tarifa);

        Task<bool> ActualizarAsync(int id, decimal nuevoPrecio);
    }
}
