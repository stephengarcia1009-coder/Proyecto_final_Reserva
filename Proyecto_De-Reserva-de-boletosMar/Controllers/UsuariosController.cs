
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.ViewModels;

namespace Proyecto_De_Reserva_de_boletosMar.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IPermisoService _permisoService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(
            IUsuarioService usuarioService,
            IPermisoService permisoService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _usuarioService = usuarioService;
            _permisoService = permisoService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();
            var rolesDisponibles = await _usuarioService.ObtenerRolesDisponiblesAsync();

            var modelo = new UsuariosGestionViewModel
            {
                RolesDisponibles = rolesDisponibles.ToList()
            };

            foreach (var usuario in usuarios)
            {
                modelo.Usuarios.Add(new UsuarioGestionItemViewModel
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Roles = (await _usuarioService.ObtenerRolesAsync(usuario.Id)).ToList(),
                    Permisos = (await _permisoService.ObtenerPermisosUsuarioAsync(usuario.Id)).ToList()
                });
            }

            var roles = _roleManager.Roles.ToList();

            foreach (var rol in roles)
            {
                modelo.RolesGestion.Add(new RolGestionItemViewModel
                {
                    Id = rol.Id,
                    Nombre = rol.Name ?? string.Empty,
                    Permisos = (await _permisoService.ObtenerPermisosRolAsync(rol.Id)).ToList()
                });
            }

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarRol(string usuarioId, string rol)
        {
            if (string.IsNullOrWhiteSpace(usuarioId) ||
                string.IsNullOrWhiteSpace(rol))
            {
                TempData["Error"] = "Debes seleccionar un usuario y un rol.";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _usuarioService.ObtenerRolesDisponiblesAsync();

            if (!roles.Contains(rol))
            {
                TempData["Error"] = "El rol seleccionado no es válido.";
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _usuarioService.AsignarRolAsync(usuarioId, rol);

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado ? "Rol asignado correctamente." : "No se pudo asignar el rol.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuitarRol(string usuarioId, string rol)
        {
            if (string.IsNullOrWhiteSpace(usuarioId) ||
                string.IsNullOrWhiteSpace(rol))
            {
                TempData["Error"] = "Datos incompletos.";
                return RedirectToAction(nameof(Index));
            }

            var usuarioActual = await _userManager.GetUserAsync(User);

            if (rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase) &&
                usuarioActual?.Id == usuarioId)
            {
                TempData["Error"] = "No puedes quitarte tu propio rol de Administrador.";
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _usuarioService.QuitarRolAsync(usuarioId, rol);

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado ? "Rol retirado correctamente." : "No se pudo retirar el rol.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarPermiso(string usuarioId, string permiso)
        {
            if (string.IsNullOrWhiteSpace(usuarioId) ||
                string.IsNullOrWhiteSpace(permiso))
            {
                TempData["Error"] = "Debes indicar el usuario y el permiso.";
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _permisoService.AsignarPermisoUsuarioAsync(
                usuarioId, permiso.Trim());

            if (resultado)
            {
                var usuario = await _userManager.FindByIdAsync(usuarioId);
                var usuarioActual = await _userManager.GetUserAsync(User);

                if (usuario != null &&
                    usuarioActual != null &&
                    usuarioActual.Id == usuarioId)
                {
                    await _signInManager.RefreshSignInAsync(usuario);
                }
            }

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado ? "Permiso asignado correctamente." : "No se pudo asignar el permiso.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuitarPermiso(string usuarioId, string permiso)
        {
            if (string.IsNullOrWhiteSpace(usuarioId) ||
                string.IsNullOrWhiteSpace(permiso))
            {
                TempData["Error"] = "Datos incompletos.";
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _permisoService.QuitarPermisoUsuarioAsync(
                usuarioId, permiso.Trim());

            if (resultado)
            {
                var usuario = await _userManager.FindByIdAsync(usuarioId);
                var usuarioActual = await _userManager.GetUserAsync(User);

                if (usuario != null &&
                    usuarioActual != null &&
                    usuarioActual.Id == usuarioId)
                {
                    await _signInManager.RefreshSignInAsync(usuario);
                }
            }

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado ? "Permiso retirado correctamente." : "No se pudo retirar el permiso.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarPermisoRol(string rolId, string permiso)
        {
            if (string.IsNullOrWhiteSpace(rolId) ||
                string.IsNullOrWhiteSpace(permiso))
            {
                TempData["Error"] = "Debes seleccionar un rol y un permiso.";
                return RedirectToAction(nameof(Index));
            }

            var rol = await _roleManager.FindByIdAsync(rolId);

            if (rol == null)
            {
                TempData["Error"] = "El rol seleccionado no existe.";
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _permisoService.AsignarPermisoRolAsync(
                rolId, permiso.Trim());

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado
                ? "Permiso asignado al rol correctamente. Los usuarios de ese rol deberán iniciar sesión nuevamente."
                : "No se pudo asignar el permiso al rol.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuitarPermisoRol(string rolId, string permiso)
        {
            if (string.IsNullOrWhiteSpace(rolId) ||
                string.IsNullOrWhiteSpace(permiso))
            {
                TempData["Error"] = "Datos incompletos.";
                return RedirectToAction(nameof(Index));
            }

            var rol = await _roleManager.FindByIdAsync(rolId);

            if (rol == null)
            {
                TempData["Error"] = "El rol seleccionado no existe.";
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _permisoService.QuitarPermisoRolAsync(
                rolId, permiso.Trim());

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado
                ? "Permiso retirado del rol correctamente. Los usuarios de ese rol deberán iniciar sesión nuevamente."
                : "No se pudo retirar el permiso del rol.";

            return RedirectToAction(nameof(Index));
        }
    }
}
