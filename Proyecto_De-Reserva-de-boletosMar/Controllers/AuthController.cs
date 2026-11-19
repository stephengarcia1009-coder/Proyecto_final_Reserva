
using Microsoft.AspNetCore.Mvc;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_De_Reserva_de_boletosMar.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Mostrar formulario de inicio de sesión
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Procesar inicio de sesión
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            var resultado = await _authService.IniciarSesionAsync(
                modelo.Correo,
                modelo.Password,
                modelo.Recordar);

            if (resultado.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty,
                "Correo o contraseña incorrectos.");

            return View(modelo);
        }

        // Mostrar formulario de registro
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // Procesar registro
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
                return View(modelo);

            var resultado = await _authService.RegistrarAsync(
                modelo.NombreCompleto,
                modelo.Correo,
                modelo.Telefono,
                modelo.Password);

            if (resultado.Succeeded)
            {
                TempData["Mensaje"] = "Registro completado correctamente.";
                return RedirectToAction("Login");
            }

            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(modelo);
        }

        // Cerrar sesión
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.CerrarSesionAsync();

            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }

}
