
using Microsoft.EntityFrameworkCore;
using Proyecto_De_Reserva_de_boletosMar.Data;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class ViajeService : IViajeService
    {
        private readonly IGenericRepository<Viaje> _repository;
        private readonly IGenericRepository<Ruta> _rutaRepository;
        private readonly IGenericRepository<Panga> _pangaRepository;
        private readonly PangaDbContext _context;

        public ViajeService(
            IGenericRepository<Viaje> repository,
            IGenericRepository<Ruta> rutaRepository,
            IGenericRepository<Panga> pangaRepository,
            PangaDbContext context)
        {
            _repository = repository;
            _rutaRepository = rutaRepository;
            _pangaRepository = pangaRepository;
            _context = context;
        }

        public async Task<IEnumerable<Viaje>> ObtenerTodosAsync()
        {
            return await _context.Viajes
                .Include(v => v.Ruta)
                .Include(v => v.Panga)
                .Include(v => v.EstadoViaje)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Viaje?> ObtenerPorIdAsync(int id)
        {
            return await _context.Viajes
                .Include(v => v.Ruta)
                .Include(v => v.Panga)
                .Include(v => v.EstadoViaje)
                .FirstOrDefaultAsync(v => v.ViajeId == id);
        }

        public async Task<IEnumerable<Viaje>> ObtenerDisponiblesAsync(
            DateTime fecha)
        {
            return await _context.Viajes
                .Include(v => v.Ruta)
                .Include(v => v.Panga)
                .Where(v =>
                    v.FechaSalida.Date == fecha.Date &&
                    v.EstadoViajeId == 1 &&
                    v.Ruta != null &&
                    v.Ruta.Activa)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> RegistrarAsync(Viaje viaje)
        {
            if (viaje.FechaSalida <= DateTime.Now)
                return false;

            bool rutaExiste = await _rutaRepository.ExistsAsync(
                r => r.RutaId == viaje.RutaId && r.Activa);

            bool pangaExiste = await _pangaRepository.ExistsAsync(
                p => p.PangaId == viaje.PangaId);

            if (!rutaExiste || !pangaExiste)
                return false;

            viaje.EstadoViajeId = 1;

            await _repository.AddAsync(viaje);

            return true;
        }

        public async Task<bool> ActualizarAsync(int id, Viaje viaje)
        {
            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
                return false;

            if (existente.EstadoViajeId != 1 ||
                viaje.FechaSalida <= DateTime.Now)
                return false;

            bool rutaExiste = await _rutaRepository.ExistsAsync(
                r => r.RutaId == viaje.RutaId && r.Activa);

            bool pangaExiste = await _pangaRepository.ExistsAsync(
                p => p.PangaId == viaje.PangaId);

            if (!rutaExiste || !pangaExiste)
                return false;

            existente.RutaId = viaje.RutaId;
            existente.PangaId = viaje.PangaId;
            existente.FechaSalida = viaje.FechaSalida;
            existente.Observaciones = viaje.Observaciones;

            await _repository.UpdateAsync(existente);

            return true;
        }

        public async Task<bool> CancelarAsync(int id)
        {
            var viaje = await _repository.GetByIdAsync(id);

            if (viaje == null || viaje.EstadoViajeId != 1)
                return false;

            viaje.EstadoViajeId = 2;

            await _repository.UpdateAsync(viaje);

            return true;
        }

       
    }
}
