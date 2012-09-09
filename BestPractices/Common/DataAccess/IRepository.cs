using System.Linq;

namespace Common.DataAccess
{
    public interface IRepository
    {
        void Add<T>(T value) where T : class;

        void Delete<T>(object key) where T : class;
        void Delete<T>(T value) where T : class;

        T Find<T>(object key) where T : class;

        void Update<T>(T value) where T : class;

        IQueryable<T> Query<T>() where T : class;

        void SaveChanges();
    }
}