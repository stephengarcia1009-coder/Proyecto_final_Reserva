
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_De_Reserva_de_boletosMar.DTOs;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TarifasController : Controller
    {
        private readonly ITarifaService _tarifaService;
        private readonly IRutaService _rutaService;

        public TarifasController(
            ITarifaService tarifaService,
            IRutaService rutaService)
        {
            _tarifaService = tarifaService;
            _rutaService = rutaService;
        }

        public async Task<IActionResult> Index()
        {
            var tarifas = await _tarifaService.ObtenerTodasAsync();
            var rutas = await _rutaService.ObtenerTodasAsync();

            ViewBag.NombresRutas = rutas.ToDictionary(
                r => r.RutaId,
                r => $"{r.Origen?.Nombre ?? "Origen"} - {r.Destino?.Nombre ?? "Destino"}"
            );

            return View(tarifas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tarifa = await _tarifaService.ObtenerPorIdAsync(id);

            if (tarifa == null)
                return NotFound();

            return View(tarifa);
        }

        private async Task CargarRutasDisponiblesAsync(int? rutaSeleccionada = null)
        {
            var rutas = await _rutaService.ObtenerTodasAsync();
            var tarifas = await _tarifaService.ObtenerTodasAsync();

            var rutasConTarifa = tarifas
                .Select(t => t.RutaId)
                .ToHashSet();

            var rutasDisponibles = rutas
                .Where(r => r.Activa && !rutasConTarifa.Contains(r.RutaId))
                .Select(r => new SelectListItem
                {
                    Value = r.RutaId.ToString(),
                    Text = $"{r.Origen?.Nombre ?? "Origen"} → {r.Destino?.Nombre ?? "Destino"}",
                    Selected = r.RutaId == rutaSeleccionada
                })
                .ToList();

            ViewBag.Rutas = rutasDisponibles;
        }

        private async Task<string> ObtenerNombreRutaAsync(int rutaId)
        {
            var rutas = await _rutaService.ObtenerTodasAsync();

            var ruta = rutas.FirstOrDefault(r => r.RutaId == rutaId);

            if (ruta == null)
                return "Ruta no encontrada";

            return $"{ruta.Origen?.Nombre ?? "Origen"} → {ruta.Destino?.Nombre ?? "Destino"}";
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

            await CargarRutasDisponiblesAsync(rutaId);

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
            {
                await CargarRutasDisponiblesAsync(tarifaDto.RutaId);
                return View(tarifaDto);
            }

            var tarifaExistente = await _tarifaService
                .ObtenerPorRutaAsync(tarifaDto.RutaId);

            if (tarifaExistente != null)
            {
                ModelState.AddModelError(nameof(tarifaDto.RutaId),
                    "Esta ruta ya tiene una tarifa registrada.");

                await CargarRutasDisponiblesAsync(tarifaDto.RutaId);
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

                await CargarRutasDisponiblesAsync(tarifaDto.RutaId);
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

            ViewBag.NombreRuta = await ObtenerNombreRutaAsync(tarifa.RutaId);

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
            {
                var tarifaActual = await _tarifaService.ObtenerPorIdAsync(id);

                if (tarifaActual == null)
                    return NotFound();

                ViewBag.NombreRuta = await ObtenerNombreRutaAsync(tarifaActual.RutaId);
                return View(tarifaDto);
            }

            var resultado = await _tarifaService.ActualizarAsync(id, tarifaDto.Precio);

            if (!resultado)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
