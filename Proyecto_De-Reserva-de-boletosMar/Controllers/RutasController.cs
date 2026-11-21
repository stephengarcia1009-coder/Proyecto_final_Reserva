
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_De_Reserva_de_boletosMar.DTOs;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RutasController : Controller
    {
        private readonly IRutaService _rutaService;

        public RutasController(IRutaService rutaService)
        {
            _rutaService = rutaService;
        }

        public async Task<IActionResult> Index()
        {
            var rutas = await _rutaService.ObtenerTodasAsync();
            return View(rutas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ruta = await _rutaService.ObtenerPorIdAsync(id);

            if (ruta == null)
                return NotFound();

            return View(ruta);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RutaDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RutaDto rutaDto)
        {
            if (!ModelState.IsValid)
                return View(rutaDto);

            var ruta = new Ruta
            {
                OrigenId = rutaDto.OrigenId,
                DestinoId = rutaDto.DestinoId,
                Descripcion = rutaDto.Descripcion,
                Activa = rutaDto.Activa
            };

            var resultado = await _rutaService.RegistrarAsync(ruta);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo registrar la ruta. Verifica que los datos sean correctos y que no esté duplicada.");

                return View(rutaDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ruta = await _rutaService.ObtenerPorIdAsync(id);

            if (ruta == null)
                return NotFound();

            var rutaDto = new RutaDto
            {
                RutaId = ruta.RutaId,
                OrigenId = ruta.OrigenId,
                DestinoId = ruta.DestinoId,
                Descripcion = ruta.Descripcion,
                Activa = ruta.Activa
            };

            return View(rutaDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RutaDto rutaDto)
        {
            if (id != rutaDto.RutaId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(rutaDto);

            var ruta = new Ruta
            {
                RutaId = rutaDto.RutaId,
                OrigenId = rutaDto.OrigenId,
                DestinoId = rutaDto.DestinoId,
                Descripcion = rutaDto.Descripcion,
                Activa = rutaDto.Activa
            };

            var resultado = await _rutaService.ActualizarAsync(id, ruta);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo actualizar la ruta.");

                return View(rutaDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var resultado = await _rutaService.CambiarEstadoAsync(id);

            if (!resultado)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
