using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VillaAPIProject.Data;
using VillaAPIProject.Model;
using VillaAPIProject.Repository.IRepository;

namespace VillaAPIProject.Repository
{
    public class Repository<T>: IRepository<T> where T: class
    {
        //implement DBContext using dependency injection
        private readonly ApplicationDbContext _db;
        //create a DB set to clean up the code instead of uisng _db.Villas
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet=_db.Set<T>();
        }


        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();

        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            //use IQueryable
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            //use IQueryable
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }
                
    }
}
