
using Microsoft.EntityFrameworkCore;
using Proyecto_De_Reserva_de_boletosMar.Data;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;
using System.Data;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class ReservaService : IReservaService
    {
        private readonly PangaDbContext _context;

        public ReservaService(PangaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reserva>> ObtenerTodasAsync()
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v!.Ruta)
                        .ThenInclude(r => r!.Origen)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v!.Ruta)
                        .ThenInclude(r => r!.Destino)
                .Include(r => r.EstadoReserva)
                .Include(r => r.DetallesReserva)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Reserva?> ObtenerPorIdAsync(int id)
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v!.Ruta)
                .Include(r => r.Viaje)
                    .ThenInclude(v => v!.Panga)
                .Include(r => r.EstadoReserva)
                .Include(r => r.DetallesReserva)
                .FirstOrDefaultAsync(r => r.ReservaId == id);
        }

        public async Task<IEnumerable<Reserva>> ObtenerPorClienteAsync(int clienteId)
        {
            return await _context.Reservas
                .Include(r => r.Viaje)
    .ThenInclude(v => v!.Ruta)
        .ThenInclude(r => r!.Origen)
.Include(r => r.Viaje)
    .ThenInclude(v => v!.Ruta)
        .ThenInclude(r => r!.Destino)
.Include(r => r.EstadoReserva)
                .Include(r => r.DetallesReserva)
                .Where(r => r.ClienteId == clienteId &&
            r.Viaje != null &&
            r.Viaje.FechaSalida >= DateTime.Now)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> RegistrarAsync(Reserva reserva)
        {


            if (reserva.CantidadBoletos < 1 ||
       reserva.CantidadBoletos > 100)
                return false;

            if (reserva.DetallesReserva == null ||
                reserva.DetallesReserva.Count != reserva.CantidadBoletos)
                return false;

            if (reserva.DetallesReserva.Any(d =>
                string.IsNullOrWhiteSpace(d.NombrePasajero) ||
                string.IsNullOrWhiteSpace(d.TelefonoPasajero)))
                return false;

            var viaje = await _context.Viajes
                .Include(v => v.Panga)
                .FirstOrDefaultAsync(v => v.ViajeId == reserva.ViajeId);

            if (viaje == null || viaje.EstadoViajeId != 1 ||
     viaje.Panga == null || viaje.FechaSalida <= DateTime.Now)
                return false;

            var ruta = await _context.Rutas
                .FirstOrDefaultAsync(r => r.RutaId == viaje.RutaId);

            if (ruta == null || !ruta.Activa)
                return false;

            await using var transaction =
       await _context.Database.BeginTransactionAsync(
           IsolationLevel.Serializable);

            var boletosOcupados = await _context.Reservas
                .Where(r => r.ViajeId == reserva.ViajeId &&
                            r.EstadoReservaId != 3)
                .SumAsync(r => (int?)r.CantidadBoletos) ?? 0;

            if (boletosOcupados + reserva.CantidadBoletos >
                viaje.Panga.Capacidad)
                return false;

            var tarifa = await _context.Tarifas
                .FirstOrDefaultAsync(t => t.RutaId == viaje.RutaId);

            if (tarifa == null || tarifa.Precio <= 0)
                return false;

            reserva.CodigoReserva = Guid.NewGuid()
                .ToString("N")[..12].ToUpper();

            reserva.FechaReserva = DateTime.Now;
            reserva.EstadoReservaId = 1;
            reserva.Total = tarifa.Precio * reserva.CantidadBoletos;

            foreach (var detalle in reserva.DetallesReserva)
            {
                detalle.PrecioUnitario = tarifa.Precio;
            }

            _context.Reservas.Add(reserva);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }

        public async Task<bool> CancelarAsync(int id)
        {
            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.ReservaId == id);

            if (reserva == null || reserva.EstadoReservaId == 3)
                return false;

            reserva.EstadoReservaId = 3;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmarAsync(int id)
        {
            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.ReservaId == id);

            if (reserva == null || reserva.EstadoReservaId != 1)
                return false;

            reserva.EstadoReservaId = 2;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
