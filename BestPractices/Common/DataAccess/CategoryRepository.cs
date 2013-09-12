using System.Linq;

namespace Common.DataAccess
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetAvailableCategories();
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetAvailableCategories()
        {
            return _context.Categories;
        }
    }
}