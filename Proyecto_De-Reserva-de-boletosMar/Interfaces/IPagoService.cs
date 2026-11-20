
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IPagoService
    {
        Task<Pago?> ObtenerPorReservaAsync(int reservaId);

        Task<bool> ProcesarPagoAsync(
            int reservaId,
            int metodoPagoId,
            bool pagoExitoso);

        Task<IEnumerable<DetallePago>> ObtenerIntentosAsync(int pagoId);
    }
}
