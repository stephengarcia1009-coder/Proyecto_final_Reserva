
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class TarifaService : ITarifaService
    {
        private readonly IGenericRepository<Tarifa> _repository;
        private readonly IGenericRepository<DetalleTarifa> _detalleRepository;

        public TarifaService(
            IGenericRepository<Tarifa> repository,
            IGenericRepository<DetalleTarifa> detalleRepository)
        {
            _repository = repository;
            _detalleRepository = detalleRepository;
        }

        public async Task<IEnumerable<Tarifa>> ObtenerTodasAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Tarifa?> ObtenerPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Tarifa?> ObtenerPorRutaAsync(int rutaId)
        {
            var tarifas = await _repository.GetAllAsync();

            return tarifas.FirstOrDefault(t => t.RutaId == rutaId);
        }

        public async Task<bool> RegistrarAsync(Tarifa tarifa)
        {
            if (tarifa.Precio <= 0)
                return false;

            bool existe = await _repository.ExistsAsync(
                t => t.RutaId == tarifa.RutaId);

            if (existe)
                return false;

            tarifa.FechaActualizacion = DateTime.Now;

            await _repository.AddAsync(tarifa);
            return true;
        }

        public async Task<bool> ActualizarAsync(
            int id, decimal nuevoPrecio)
        {
            if (nuevoPrecio <= 0)
                return false;

            var tarifa = await _repository.GetByIdAsync(id);

            if (tarifa == null)
                return false;

            if (tarifa.Precio == nuevoPrecio)
                return false;

            var detalle = new DetalleTarifa
            {
                TarifaId = tarifa.TarifaId,
                PrecioAnterior = tarifa.Precio,
                PrecioNuevo = nuevoPrecio,
                FechaCambio = DateTime.Now
            };

            await _detalleRepository.AddAsync(detalle);

            tarifa.Precio = nuevoPrecio;
            tarifa.FechaActualizacion = DateTime.Now;

            await _repository.UpdateAsync(tarifa);

            return true;
        }
    }
}
