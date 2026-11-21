
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_De_Reserva_de_boletosMar.DTOs;
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

            return View(new TarifaDto
            {
                RutaId = rutaId ?? 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TarifaDto tarifaDto)
        {
            if (!ModelState.IsValid)
                return View(tarifaDto);

            var tarifaExistente = await _tarifaService
                .ObtenerPorRutaAsync(tarifaDto.RutaId);

            if (tarifaExistente != null)
            {
                ModelState.AddModelError(nameof(tarifaDto.RutaId),
                    "Esta ruta ya tiene una tarifa registrada.");

                return View(tarifaDto);
            }

            var tarifa = new Tarifa
            {
                RutaId = tarifaDto.RutaId,
                Precio = tarifaDto.Precio,
                FechaActualizacion = DateTime.Now
            };

            var resultado = await _tarifaService.RegistrarAsync(tarifa);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo registrar la tarifa.");

                return View(tarifaDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tarifa = await _tarifaService.ObtenerPorIdAsync(id);

            if (tarifa == null)
                return NotFound();

            var tarifaDto = new TarifaDto
            {
                TarifaId = tarifa.TarifaId,
                RutaId = tarifa.RutaId,
                Precio = tarifa.Precio,
                FechaActualizacion = tarifa.FechaActualizacion
            };

            return View(tarifaDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TarifaDto tarifaDto)
        {
            if (id != tarifaDto.TarifaId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(tarifaDto);

            var resultado = await _tarifaService.ActualizarAsync(id, tarifaDto.Precio);

            if (!resultado)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
