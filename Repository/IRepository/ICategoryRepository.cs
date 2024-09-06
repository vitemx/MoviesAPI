using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.Models;

namespace MoviesAPI.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        bool CategoryExits(int categoryId);
        bool CategoryExits(string categoryName);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DelteCategory(Category category);
        bool Save();
    }
}