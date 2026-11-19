
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;
using Proyecto_De_Reserva_de_boletosMar.ViewModels;


namespace Proyecto_De_Reserva_de_boletosMar.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly IReservaService _reservaService;
        private readonly IClienteService _clienteService;
        private readonly IViajeService _viajeService;

        public ReservasController(
    IReservaService reservaService,
    IClienteService clienteService,
    IViajeService viajeService)
        {
            _reservaService = reservaService;
            _clienteService = clienteService;
            _viajeService = viajeService;
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            var reservas = await _reservaService.ObtenerTodasAsync();
            return View(reservas);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int id)


        {
            var reserva = await _reservaService.ObtenerPorIdAsync(id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> CrearPresencial()
        {
            var clientes = await _clienteService.ObtenerTodosAsync();

            var viajes = await _viajeService.ObtenerDisponiblesAsync(
                DateTime.Today);

            ViewBag.Clientes = clientes;
            ViewBag.Viajes = viajes;

            var modelo = new ReservaPresencialViewModel
            {
                Pasajeros = new List<PasajeroViewModel>
        {
            new PasajeroViewModel()
        }
            };

            return View(modelo);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPresencial(
            ReservaPresencialViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = await _clienteService.ObtenerTodosAsync();
                ViewBag.Viajes = await _viajeService.ObtenerDisponiblesAsync(
                    DateTime.Today);

                return View(modelo);
            }

            var reserva = new Reserva
            {
                ClienteId = modelo.ClienteId,
                ViajeId = modelo.ViajeId,
                CantidadBoletos = modelo.Pasajeros.Count,
                DetallesReserva = modelo.Pasajeros.Select(p => new DetalleReserva
                {
                    NombrePasajero = p.NombrePasajero,
                    TelefonoPasajero = p.TelefonoPasajero
                }).ToList()
            };

            var resultado = await _reservaService.RegistrarAsync(reserva);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No fue posible registrar la reserva. Verifica la capacidad y la tarifa.");

                ViewBag.Clientes = await _clienteService.ObtenerTodosAsync();
                ViewBag.Viajes = await _viajeService.ObtenerDisponiblesAsync(
                    DateTime.Today);

                return View(modelo);
            }

            TempData["Mensaje"] = "Reserva presencial registrada correctamente. Está pendiente de pago.";

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> MisReservas()
        {

            var usuarioId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized();

            var cliente = await _clienteService.ObtenerPorUsuarioAsync(usuarioId);

            if (cliente == null)
                return NotFound();

            var reservas = await _reservaService.ObtenerPorClienteAsync(
                cliente.ClienteId);

            return View(reservas);


        }

        [Authorize(Roles = "Cliente")]
        [HttpGet]
        public async Task<IActionResult> SeleccionarViaje(DateTime? fecha)
        {
            var fechaConsulta = fecha?.Date ?? DateTime.Today;

            var viajes = await _viajeService.ObtenerDisponiblesAsync(
                fechaConsulta);

            ViewBag.FechaConsulta = fechaConsulta.ToString("yyyy-MM-dd");

            return View(viajes);
        }
        [Authorize(Roles = "Cliente")]
        [HttpGet]
        public async Task<IActionResult> Crear(int viajeId)
        {
            var viaje = await _viajeService.ObtenerPorIdAsync(viajeId);

            if (viaje == null || viaje.EstadoViajeId != 1 ||
                viaje.FechaSalida <= DateTime.Now)
            {
                return NotFound();
            }

            var viewModel = new ReservaViewModel
            {
                ViajeId = viajeId,
                Pasajeros = new List<PasajeroViewModel>
        {
            new PasajeroViewModel()
        }
            };

            ViewBag.Viaje = viaje;

            return View(viewModel);
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ReservaViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Viaje = await _viajeService.ObtenerPorIdAsync(modelo.ViajeId);
                return View(modelo);
            }

            var usuarioId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioId))
                return Unauthorized();

            var cliente = await _clienteService.ObtenerPorUsuarioAsync(usuarioId);

            if (cliente == null)
                return NotFound();

            var reserva = new Reserva
            {
                ClienteId = cliente.ClienteId,
                ViajeId = modelo.ViajeId,
                CantidadBoletos = modelo.Pasajeros.Count,
                DetallesReserva = modelo.Pasajeros.Select(p => new DetalleReserva
                {
                    NombrePasajero = p.NombrePasajero,
                    TelefonoPasajero = p.TelefonoPasajero
                }).ToList()
            };

            var resultado = await _reservaService.RegistrarAsync(reserva);

            if (!resultado)
            {
                ModelState.AddModelError(string.Empty,
                    "No fue posible registrar la reserva. Verifica que haya capacidad y una tarifa disponible.");

                ViewBag.Viaje = await _viajeService.ObtenerPorIdAsync(modelo.ViajeId);
                return View(modelo);
            }

            TempData["Mensaje"] = "Reserva registrada correctamente. Está pendiente de pago.";

            return RedirectToAction(nameof(MisReservas));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            var reserva = await _reservaService.ObtenerPorIdAsync(id);

            if (reserva == null)
                return NotFound();

            if (User.IsInRole("Cliente"))
            {
                var usuarioId = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                var cliente = await _clienteService.ObtenerPorUsuarioAsync(usuarioId!);

                if (cliente == null || reserva.ClienteId != cliente.ClienteId)
                    return Forbid();

                if (reserva.EstadoReservaId != 1)
                    return Forbid();
            }
            else if (User.IsInRole("Administrador"))
            {
                if (reserva.EstadoReservaId == 3)
                    return BadRequest();
            }
            else
            {
                return Forbid();
            }

            var resultado = await _reservaService.CancelarAsync(id);

            if (!resultado)
                return BadRequest();

            TempData["Mensaje"] = "La reserva fue cancelada correctamente.";

            if (User.IsInRole("Administrador"))
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(MisReservas));
        }

    }
}

