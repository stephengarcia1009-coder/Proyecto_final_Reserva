
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TarifasController : Controller
    {
        private readonly ITarifaService _tarifaService;

        public TarifasController(ITarifaService tarifaService)
        {
            _tarifaService = tarifaService;
        }

        public async Task<IActionResult> Index()
        {
            var tarifas = await _tarifaService.ObtenerTodasAsync();
            return View(tarifas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tarifa = await _tarifaService.ObtenerPorIdAsync(id);

            if (tarifa == null)
                return NotFound();

            return View(tarifa);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? rutaId)
        {
            if (rutaId.HasValue)
            {
                var tarifaExistente = await _tarifaService
                    .ObtenerPorRutaAsync(rutaId.Value);

                if (tarifaExistente != null)
                {
                    TempData["Error"] = "Esta ruta ya tiene una tarifa registrada.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tarifa tarifa)
        {
            if (!ModelState.IsValid)
                return View(tarifa);

            var tarifaExistente = await _tarifaService
                .ObtenerPorRutaAsync(tarifa.RutaId);

            if (tarifaExistente != null)
            {
                ModelState.AddModelError(nameof(tarifa.RutaId),
                    "Esta ruta ya tiene una tarifa registrada.");

                return View(tarifa);
            }

            var resultado = await _tarifaService.RegistrarAsync(tarifa);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo registrar la tarifa.");

                return View(tarifa);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tarifa = await _tarifaService.ObtenerPorIdAsync(id);

            if (tarifa == null)
                return NotFound();

            return View(tarifa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, decimal nuevoPrecio)
        {
            if (nuevoPrecio <= 0)
            {
                ModelState.AddModelError(nameof(nuevoPrecio),
                    "El precio debe ser mayor que cero.");

                var tarifaActual = await _tarifaService.ObtenerPorIdAsync(id);

                if (tarifaActual == null)
                    return NotFound();

                return View(tarifaActual);
            }

            var resultado = await _tarifaService.ActualizarAsync(id, nuevoPrecio);

            if (!resultado)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
