
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_De_Reserva_de_boletosMar.DTOs;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Controllers
{
    [Authorize]
    public class ViajesController : Controller
    {
        private readonly IViajeService _viajeService;
        private readonly IRutaService _rutaService;
        private readonly IPangaService _pangaService;

        public ViajesController(
            IViajeService viajeService,
            IRutaService rutaService,
            IPangaService pangaService)
        {
            _viajeService = viajeService;
            _rutaService = rutaService;
            _pangaService = pangaService;
        }

        private async Task CargarListasAsync()
        {
            var rutas = await _rutaService.ObtenerTodasAsync();
            var pangas = await _pangaService.ObtenerTodasAsync();

            ViewBag.Rutas = rutas
                .Where(r => r.Activa)
                .Select(r => new SelectListItem
                {
                    Value = r.RutaId.ToString(),
                    Text = $"{r.Origen?.Nombre ?? "Origen"} - {r.Destino?.Nombre ?? "Destino"}"
                })
                .ToList();

            ViewBag.Pangas = new SelectList(
                pangas,
                "PangaId",
                "Nombre");
        }

        [Authorize(Policy = "Viajes.Ver")]
        public async Task<IActionResult> Index()
        {
            var viajes = await _viajeService.ObtenerTodosAsync();
            return View(viajes);
        }

        [Authorize(Policy = "Viajes.Ver")]
        public async Task<IActionResult> Details(int id)
        {
            var viaje = await _viajeService.ObtenerPorIdAsync(id);

            if (viaje == null)
                return NotFound();

            return View(viaje);
        }

        [Authorize(Policy = "Viajes.Gestionar.Acceso")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CargarListasAsync();
            return View(new ViajeDto());
        }

        [Authorize(Policy = "Viajes.Gestionar.Acceso")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViajeDto viajeDto)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasAsync();
                return View(viajeDto);
            }

            var viaje = new Viaje
            {
                RutaId = viajeDto.RutaId,
                PangaId = viajeDto.PangaId,
                FechaSalida = viajeDto.FechaSalida,
                Observaciones = viajeDto.Observaciones
            };

            var resultado = await _viajeService.RegistrarAsync(viaje);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo registrar el viaje. Verifica los datos y que la ruta esté activa y la fecha sea futura.");

                await CargarListasAsync();
                return View(viajeDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Viajes.Gestionar.Acceso")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var viaje = await _viajeService.ObtenerPorIdAsync(id);

            if (viaje == null)
                return NotFound();

            var viajeDto = new ViajeDto
            {
                ViajeId = viaje.ViajeId,
                RutaId = viaje.RutaId,
                PangaId = viaje.PangaId,
                FechaSalida = viaje.FechaSalida,
                Observaciones = viaje.Observaciones
            };

            await CargarListasAsync();

            return View(viajeDto);
        }

        [Authorize(Policy = "Viajes.Gestionar.Acceso")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ViajeDto viajeDto)
        {
            if (id != viajeDto.ViajeId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await CargarListasAsync();
                return View(viajeDto);
            }

            var viaje = new Viaje
            {
                ViajeId = viajeDto.ViajeId,
                RutaId = viajeDto.RutaId,
                PangaId = viajeDto.PangaId,
                FechaSalida = viajeDto.FechaSalida,
                Observaciones = viajeDto.Observaciones
            };

            var resultado = await _viajeService.ActualizarAsync(id, viaje);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo actualizar el viaje. Verifica los datos y que la fecha sea futura.");

                await CargarListasAsync();
                return View(viajeDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Viajes.Gestionar.Acceso")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            var resultado = await _viajeService.CancelarAsync(id);

            if (!resultado)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

       
    }
}
