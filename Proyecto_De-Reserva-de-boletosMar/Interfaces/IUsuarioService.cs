
using Microsoft.AspNetCore.Identity;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<IdentityUser>> ObtenerTodosAsync();

        Task<IdentityUser?> ObtenerPorIdAsync(string usuarioId);

        Task<IList<string>> ObtenerRolesAsync(string usuarioId);

        Task<bool> AsignarRolAsync(string usuarioId, string rol);

        Task<bool> QuitarRolAsync(string usuarioId, string rol);
    }
}
