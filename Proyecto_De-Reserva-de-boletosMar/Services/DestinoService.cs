
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class DestinoService : IDestinoService
    {
        private readonly IGenericRepository<Destino> _repository;

        public DestinoService(IGenericRepository<Destino> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Destino>> ObtenerTodosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Destino?> ObtenerPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RegistrarAsync(Destino destino)
        {
            bool existe = await _repository.ExistsAsync(
                d => d.Nombre == destino.Nombre);

            if (existe)
                return false;

            await _repository.AddAsync(destino);
            return true;
        }

        public async Task<bool> ActualizarAsync(int id, Destino destino)
        {
            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
                return false;

            existente.Nombre = destino.Nombre;
            existente.Descripcion = destino.Descripcion;
            existente.Activo = destino.Activo;

            await _repository.UpdateAsync(existente);
            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int id)
        {
            var destino = await _repository.GetByIdAsync(id);

            if (destino == null)
                return false;

            destino.Activo = !destino.Activo;

            await _repository.UpdateAsync(destino);
            return true;
        }
    }
}
