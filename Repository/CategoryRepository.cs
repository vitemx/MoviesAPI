using MoviesAPI.Data;
using MoviesAPI.Models;
using MoviesAPI.Repository.IRepository;

namespace MoviesAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CategoryExits(int categoryId)
        {
            return _dbContext.Category.Any(c => c.Id == categoryId);
        }

        public bool CategoryExits(string categoryName)
        {
            bool exits = _dbContext.Category.Any(c => c.Name.ToUpper().Trim() == categoryName.ToUpper().Trim());
            return exits;
        }

        public bool CreateCategory(Category category)
        {
            category.CreationDate = DateTime.Now;
            _dbContext.Category.Add(category);
            return Save();
        }

        public bool DelteCategory(Category category)
        {
            _dbContext.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _dbContext.Category.OrderBy(x => x.Name).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _dbContext.Category.FirstOrDefault(c => c.Id == categoryId) ?? new Category();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            category.CreationDate = DateTime.Now;
            _dbContext.Update(category);
            return Save();
        }
    }
}