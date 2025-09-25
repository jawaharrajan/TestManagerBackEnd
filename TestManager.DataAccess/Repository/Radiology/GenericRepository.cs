using TestManager.DataAccess.Repository.Contracts;
using TestManager.Domain.Model;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;


namespace TestManager.DataAccess.Repository.Radiology
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>  where TEntity : BaseEntity<TKey>
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<TEntity> GetByIdAsync(TKey id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(TEntity entity)
        {
            //entity.CreatedAt = DateTime.UtcNow;
            //entity.UpdatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            //entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await _dbSet.FindAsync(id); 
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<TResult?> GetProjectedByIdAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector)
        {
            return await _context.Set<TEntity>()
                .Where(predicate)
                .Select(selector)
                .FirstOrDefaultAsync();
        }

    }
}
