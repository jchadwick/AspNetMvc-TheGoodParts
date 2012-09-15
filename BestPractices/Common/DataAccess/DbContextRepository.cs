using System.Data.Entity;
using System.Linq;

namespace Common.DataAccess
{
    public class DbContextRepository : IRepository
    {
        private readonly DbContext _context;

        public DbContextRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T value) where T : class
        {
            _context.Set<T>().Add(value);
        }

        public void Delete<T>(object key) where T : class
        {
            var dbSet = _context.Set<T>();
            var toDelete = dbSet.Find(key);

            if (toDelete != null)
                dbSet.Remove(toDelete);

            _context.SaveChanges();
        }

        public void Delete<T>(T value) where T : class
        {
            _context.Set<T>().Remove(value);
            _context.SaveChanges();
        }

        public T Find<T>(object key) where T : class
        {
            return _context.Set<T>().Find(key);
        }

        public void Update<T>(T value) where T : class
        {
            _context.Set<T>().Attach(value);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }


}