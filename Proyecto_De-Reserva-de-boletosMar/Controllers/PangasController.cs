
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Panga panga)
        {
            if (!ModelState.IsValid)
                return View(panga);

            var resultado = await _pangaService.RegistrarAsync(panga);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo registrar la panga. Verifica los datos y que el nombre no esté duplicado.");

                return View(panga);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var panga = await _pangaService.ObtenerPorIdAsync(id);

            if (panga == null)
                return NotFound();

            return View(panga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Panga panga)
        {
            if (id != panga.PangaId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(panga);

            var resultado = await _pangaService.ActualizarAsync(id, panga);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No se pudo actualizar la panga.");

                return View(panga);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
