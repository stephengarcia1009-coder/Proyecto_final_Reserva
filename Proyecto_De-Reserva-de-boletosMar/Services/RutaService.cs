
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;
using Microsoft.EntityFrameworkCore;
using Proyecto_De_Reserva_de_boletosMar.Data;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class RutaService : IRutaService
    {
        private readonly IGenericRepository<Ruta> _repository;
        private readonly PangaDbContext _context;

        public RutaService(
            IGenericRepository<Ruta> repository,
            PangaDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<Ruta>> ObtenerTodasAsync()
        {
            return await _context.Rutas
                .AsNoTracking()
                .Include(r => r.Origen)
                .Include(r => r.Destino)
                .ToListAsync();
        }
        public async Task<Ruta?> ObtenerPorIdAsync(int id)
        {
            return await _context.Rutas
                .AsNoTracking()
                .Include(r => r.Origen)
                .Include(r => r.Destino)
                .FirstOrDefaultAsync(r => r.RutaId == id);
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
