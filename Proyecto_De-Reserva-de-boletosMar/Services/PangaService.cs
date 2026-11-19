
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class PangaService : IPangaService
    {
        private readonly IGenericRepository<Panga> _repository;

        public PangaService(IGenericRepository<Panga> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Panga>> ObtenerTodasAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Panga?> ObtenerPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RegistrarAsync(Panga panga)
        {
            if (panga.Capacidad <= 0)
                return false;

            bool existe = await _repository.ExistsAsync(
                p => p.Nombre == panga.Nombre);

            if (existe)
                return false;

            await _repository.AddAsync(panga);
            return true;
        }

        public async Task<bool> ActualizarAsync(int id, Panga panga)
        {
            if (panga.Capacidad <= 0)
                return false;

            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
                return false;

            bool nombreDuplicado = await _repository.ExistsAsync(
                p => p.PangaId != id &&
                     p.Nombre == panga.Nombre);

            if (nombreDuplicado)
                return false;

            existente.Nombre = panga.Nombre;
            existente.Capacidad = panga.Capacidad;
            existente.Descripcion = panga.Descripcion;

            await _repository.UpdateAsync(existente);
            return true;
        }
    }
}
