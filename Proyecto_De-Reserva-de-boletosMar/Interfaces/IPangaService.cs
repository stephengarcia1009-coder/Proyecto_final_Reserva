
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IPangaService
    {
        Task<IEnumerable<Panga>> ObtenerTodasAsync();

        Task<Panga?> ObtenerPorIdAsync(int id);

        Task<bool> RegistrarAsync(Panga panga);

        Task<bool> ActualizarAsync(int id, Panga panga);
    }
}
