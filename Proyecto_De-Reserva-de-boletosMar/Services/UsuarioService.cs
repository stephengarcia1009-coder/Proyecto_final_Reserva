
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<IdentityUser>> ObtenerTodosAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityUser?> ObtenerPorIdAsync(string usuarioId)
        {
            return await _userManager.FindByIdAsync(usuarioId);
        }

        public async Task<IList<string>> ObtenerRolesAsync(string usuarioId)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);

            if (usuario == null)
                return new List<string>();

            return await _userManager.GetRolesAsync(usuario);
        }

        public async Task<bool> AsignarRolAsync(string usuarioId, string rol)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);

            if (usuario == null)
                return false;

            var resultado = await _userManager.AddToRoleAsync(usuario, rol);

            return resultado.Succeeded;
        }


        public async Task<bool> QuitarRolAsync(string usuarioId, string rol)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);

            if (usuario == null)
                return false;

            if (rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
            {
                var administradores = await _userManager.GetUsersInRoleAsync("Administrador");

                if (administradores.Count <= 1)
                    return false;
            }

            var resultado = await _userManager.RemoveFromRoleAsync(usuario, rol);

            return resultado.Succeeded;
        }


        public async Task<IEnumerable<string>> ObtenerRolesDisponiblesAsync()
        {
            return await _roleManager.Roles
                .Select(r => r.Name!)
                .ToListAsync();
        }
    }
}
