
using System.Data;
using Microsoft.EntityFrameworkCore;
using Proyecto_De_Reserva_de_boletosMar.Data;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class PagoService : IPagoService
    {
        private readonly PangaDbContext _context;

        public PagoService(PangaDbContext context)
        {
            _context = context;
        }

        public async Task<Pago?> ObtenerPorReservaAsync(int reservaId)
        {
            return await _context.Pagos
                .Include(p => p.MetodoPago)
                .Include(p => p.Reserva)
                .FirstOrDefaultAsync(p => p.ReservaId == reservaId);
        }

        public async Task<IEnumerable<DetallePago>> ObtenerIntentosAsync(int pagoId)
        {
            return await _context.DetallesPago
                .Where(d => d.PagoId == pagoId)
                .OrderByDescending(d => d.FechaIntento)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ProcesarPagoAsync(
            int reservaId,
            int metodoPagoId,
            bool pagoExitoso)
        {
            if (metodoPagoId != 1 && metodoPagoId != 2)
                return false;

            // El pago en efectivo solo puede registrarse como exitoso.
            if (metodoPagoId == 1 && !pagoExitoso)
                return false;

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.ReservaId == reservaId);

            if (reserva == null ||
                reserva.EstadoReservaId != 1 ||
                reserva.Total <= 0)
                return false;

            var metodoExiste = await _context.MetodosPago
                .AnyAsync(m => m.MetodoPagoId == metodoPagoId);

            if (!metodoExiste)
                return false;

            var pago = await _context.Pagos
                .FirstOrDefaultAsync(p => p.ReservaId == reservaId);

            if (pago == null)
            {
                pago = new Pago
                {
                    ReservaId = reservaId,
                    MetodoPagoId = metodoPagoId,
                    FechaPago = DateTime.Now,
                    Monto = reserva.Total,
                    Estado = "Pendiente"
                };

                _context.Pagos.Add(pago);
            }
            else
            {
                // No permitir cambiar el método de pago en los reintentos.
                if (pago.MetodoPagoId != metodoPagoId ||
                    pago.Estado == "Aprobado" ||
                    pago.Monto != reserva.Total)
                    return false;
            }

            var intento = new DetallePago
            {
                Pago = pago,
                FechaIntento = DateTime.Now,
                Monto = reserva.Total,
                Resultado = pagoExitoso ? "Aprobado" : "Rechazado",
                Observacion = metodoPagoId == 1
                    ? "Pago presencial en efectivo."
                    : pagoExitoso
                        ? "Pago simulado exitoso."
                        : "Pago simulado rechazado."
            };

            _context.DetallesPago.Add(intento);

            pago.Estado = pagoExitoso ? "Aprobado" : "Rechazado";
            pago.FechaPago = DateTime.Now;

            if (pagoExitoso)
            {
                reserva.EstadoReservaId = 2;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
    }
}
