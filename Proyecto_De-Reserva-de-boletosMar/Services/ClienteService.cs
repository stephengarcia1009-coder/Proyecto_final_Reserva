
using Microsoft.EntityFrameworkCore;
using Proyecto_De_Reserva_de_boletosMar.Data;
using Proyecto_De_Reserva_de_boletosMar.Interfaces;
using Proyecto_De_Reserva_de_boletosMar.Models;

namespace Proyecto_De_Reserva_de_boletosMar.Services
{
    public class ClienteService : IClienteService
    {
        private readonly PangaDbContext _context;
        private readonly IGenericRepository<Cliente> _repository;

        public ClienteService(
            PangaDbContext context,
            IGenericRepository<Cliente> repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<IEnumerable<Cliente>> ObtenerTodosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Cliente?> ObtenerPorIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Cliente?> ObtenerPorUsuarioAsync(string usuarioId)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<bool> ActualizarAsync(int id, Cliente cliente)
        {
            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
                return false;

            existente.NombreCompleto = cliente.NombreCompleto;
            existente.Telefono = cliente.Telefono;

            await _repository.UpdateAsync(existente);

            return true;
        }
    }
}
