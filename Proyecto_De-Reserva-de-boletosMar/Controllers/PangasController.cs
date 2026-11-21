
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_De_Reserva_de_boletosMar.DTOs;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PangasController : Controller
    {
        private readonly IPangaService _pangaService;

        public PangasController(IPangaService pangaService)
        {
            _pangaService = pangaService;
        }

        public async Task<IActionResult> Index()
        {
            var pangas = await _pangaService.ObtenerTodasAsync();
            return View(pangas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var panga = await _pangaService.ObtenerPorIdAsync(id);

            if (panga == null)
                return NotFound();

            return View(panga);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PangaDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PangaDto pangaDto)
        {
            if (!ModelState.IsValid)
                return View(pangaDto);

            var panga = new Panga
            {
                Nombre = pangaDto.Nombre,
                Capacidad = pangaDto.Capacidad,
                Descripcion = pangaDto.Descripcion
            };

            var resultado = await _pangaService.RegistrarAsync(panga);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo registrar la panga. Verifica los datos y que el nombre no esté duplicado.");

                return View(pangaDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var panga = await _pangaService.ObtenerPorIdAsync(id);

            if (panga == null)
                return NotFound();

            var pangaDto = new PangaDto
            {
                PangaId = panga.PangaId,
                Nombre = panga.Nombre,
                Capacidad = panga.Capacidad,
                Descripcion = panga.Descripcion
            };

            return View(pangaDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PangaDto pangaDto)
        {
            if (id != pangaDto.PangaId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(pangaDto);

            var panga = new Panga
            {
                PangaId = pangaDto.PangaId,
                Nombre = pangaDto.Nombre,
                Capacidad = pangaDto.Capacidad,
                Descripcion = pangaDto.Descripcion
            };

            var resultado = await _pangaService.ActualizarAsync(id, panga);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo actualizar la panga.");

                return View(pangaDto);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
