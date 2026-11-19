
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class PermisoService : IPermisoService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private const string TipoPermiso = "Permiso";

        public PermisoService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<string>> ObtenerPermisosUsuarioAsync(
            string usuarioId)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);

            if (usuario == null)
                return Enumerable.Empty<string>();

            var claims = await _userManager.GetClaimsAsync(usuario);

            return claims
                .Where(c => c.Type == TipoPermiso)
                .Select(c => c.Value);
        }

        public async Task<IEnumerable<string>> ObtenerPermisosRolAsync(
            string rolId)
        {
            var rol = await _roleManager.FindByIdAsync(rolId);

            if (rol == null)
                return Enumerable.Empty<string>();

            var claims = await _roleManager.GetClaimsAsync(rol);

            return claims
                .Where(c => c.Type == TipoPermiso)
                .Select(c => c.Value);
        }

        public async Task<bool> AsignarPermisoUsuarioAsync(
            string usuarioId, string permiso)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);

            if (usuario == null || string.IsNullOrWhiteSpace(permiso))
                return false;

            var claims = await _userManager.GetClaimsAsync(usuario);

            if (claims.Any(c =>
                c.Type == TipoPermiso && c.Value == permiso))
                return false;

            var resultado = await _userManager.AddClaimAsync(
                usuario, new Claim(TipoPermiso, permiso));

            return resultado.Succeeded;
        }

        public async Task<bool> QuitarPermisoUsuarioAsync(
            string usuarioId, string permiso)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);

            if (usuario == null || string.IsNullOrWhiteSpace(permiso))
                return false;

            var claim = new Claim(TipoPermiso, permiso);

            var resultado = await _userManager.RemoveClaimAsync(
                usuario, claim);

            return resultado.Succeeded;
        }

        public async Task<bool> AsignarPermisoRolAsync(
            string rolId, string permiso)
        {
            var rol = await _roleManager.FindByIdAsync(rolId);

            if (rol == null || string.IsNullOrWhiteSpace(permiso))
                return false;

            var claims = await _roleManager.GetClaimsAsync(rol);

            if (claims.Any(c =>
                c.Type == TipoPermiso && c.Value == permiso))
                return false;

            var resultado = await _roleManager.AddClaimAsync(
                rol, new Claim(TipoPermiso, permiso));

            return resultado.Succeeded;
        }

        public async Task<bool> QuitarPermisoRolAsync(
            string rolId, string permiso)
        {
            var rol = await _roleManager.FindByIdAsync(rolId);

            if (rol == null || string.IsNullOrWhiteSpace(permiso))
                return false;

            var claim = new Claim(TipoPermiso, permiso);

            var resultado = await _roleManager.RemoveClaimAsync(
                rol, claim);

            return resultado.Succeeded;
        }
    }
}
