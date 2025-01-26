using ControlStoreAPI.Data.Interface;
using ControlStoreAPI.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ControlStoreAPI.Data
{
    public class Repository<T> : IRepository<T> where T : class, IIdentifiable
    {
        private readonly APIDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(APIDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetItems()
        {
            return _dbSet == null ? new List<T>() : await _dbSet.ToListAsync();
        }

        public async Task<T> GetItem(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GenericQuery(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public async Task Put(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<T> Post(T item)
        {
            _dbSet.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task Delete(int id)
        {
            var item = await _dbSet.FindAsync(id);
            if (item != null)
            {
                _dbSet.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async void RemoveContex(T item)
        {
            _dbSet.Remove(item);
        }

        public async void Detached(T item)
        {
            _context.Entry(item).State = EntityState.Detached;
        }
        public async Task<bool> Exists(int id)
        {
            //return await _dbSet.FindAsync(id) != null;
            return await _dbSet.AsNoTracking().AnyAsync(x => x.ID == id);
        }

        public IQueryable<T> Query()
        {
            return _dbSet;
        }

        public int GetLasdOrOne()
        {
            int lastId = 1;

            if (_dbSet.Any())
                lastId = _dbSet.Max(u => u.ID) + 1;

            return lastId;
        }
    }
}
