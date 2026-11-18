
using System.Linq.Expressions;

namespace Proyecto_De_Reserva_de_boletosMar.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}

