
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DestinosController : Controller
    {
        private readonly IDestinoService _destinoService;

        public DestinosController(IDestinoService destinoService)
        {
            _destinoService = destinoService;
        }

        public async Task<IActionResult> Index()
        {
            var destinos = await _destinoService.ObtenerTodosAsync();
            return View(destinos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var destino = await _destinoService.ObtenerPorIdAsync(id);

            if (destino == null)
                return NotFound();

            return View(destino);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Destino destino)
        {
            if (!ModelState.IsValid)
                return View(destino);

            var resultado = await _destinoService.RegistrarAsync(destino);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo registrar el destino. Verifica que no esté duplicado.");
                return View(destino);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var destino = await _destinoService.ObtenerPorIdAsync(id);

            if (destino == null)
                return NotFound();

            return View(destino);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Destino destino)
        {
            if (id != destino.DestinoId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(destino);

            var resultado = await _destinoService.ActualizarAsync(id, destino);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo actualizar el destino.");
                return View(destino);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var resultado = await _destinoService.CambiarEstadoAsync(id);

            if (!resultado)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
