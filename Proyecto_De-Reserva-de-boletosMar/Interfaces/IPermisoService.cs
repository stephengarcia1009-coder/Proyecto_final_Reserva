
namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IPermisoService
    {
        Task<IEnumerable<string>> ObtenerPermisosUsuarioAsync(string usuarioId);

        Task<IEnumerable<string>> ObtenerPermisosRolAsync(string rolId);

        Task<bool> AsignarPermisoUsuarioAsync(string usuarioId, string permiso);

        Task<bool> QuitarPermisoUsuarioAsync(string usuarioId, string permiso);

        Task<bool> AsignarPermisoRolAsync(string rolId, string permiso);

        Task<bool> QuitarPermisoRolAsync(string rolId, string permiso);
    }
}

