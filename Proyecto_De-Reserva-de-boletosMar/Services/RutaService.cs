
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class RutaService : IRutaService
    {
        private readonly IGenericRepository<Ruta> _repository;

        public RutaService(IGenericRepository<Ruta> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Ruta>> ObtenerTodasAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Ruta?> ObtenerPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RegistrarAsync(Ruta ruta)
        {
            if (ruta.OrigenId == ruta.DestinoId)
                return false;

            bool existe = await _repository.ExistsAsync(
                r => r.OrigenId == ruta.OrigenId &&
                     r.DestinoId == ruta.DestinoId);

            if (existe)
                return false;

            await _repository.AddAsync(ruta);
            return true;
        }

        public async Task<bool> ActualizarAsync(int id, Ruta ruta)
        {
            if (ruta.OrigenId == ruta.DestinoId)
                return false;

            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
                return false;

            bool duplicada = await _repository.ExistsAsync(
                r => r.RutaId != id &&
                     r.OrigenId == ruta.OrigenId &&
                     r.DestinoId == ruta.DestinoId);

            if (duplicada)
                return false;

            existente.OrigenId = ruta.OrigenId;
            existente.DestinoId = ruta.DestinoId;
            existente.Descripcion = ruta.Descripcion;
            existente.Activa = ruta.Activa;

            await _repository.UpdateAsync(existente);
            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int id)
        {
            var ruta = await _repository.GetByIdAsync(id);

            if (ruta == null)
                return false;

            ruta.Activa = !ruta.Activa;

            await _repository.UpdateAsync(ruta);
            return true;
        }
    }
}
